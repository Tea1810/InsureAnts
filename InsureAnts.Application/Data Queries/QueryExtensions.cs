using System.Linq.Expressions;
using InsureAnts.Application.DataAccess;

namespace InsureAnts.Application.Data_Queries;

public static class QueryExtensions
{
    #region Public methods
    public static Task<QueryResult<T>> GetResultAsync<T>(this IQueryable<T> query, IQueryRequest<T> data, CancellationToken cancellationToken = default) =>
        GetResultInternalAsync(query, data, cancellationToken);

    public static QueryResult<T> GetResult<T>(this IEnumerable<T> query, IQueryRequest<T> data) => GetResultInternal(query.AsQueryable(), data);

    public static IQueryable<T> Apply<T>(this IQueryable<T> query, IQueryRequest<T> data, bool applyFiltering = true, bool applySorting = true, bool applyPagination = true)
    {
        if (applyFiltering)
        {
            query = data.ApplyFilter(query);
        }

        if (applySorting)
        {
            query = data.ApplySort(query);
        }

        if (applyPagination)
        {
            query = data.ApplySkipTake(query);
        }

        return query;
    }

    internal static IQueryable<T> ApplySort<T>(this IQueryable<T> source, IDictionary<string, bool> sorts)
    {
        var query = source;

        var parameterExpression = Expression.Parameter(typeof(T), "item");

        var isFirst = true;
        foreach (var (propertyName, isAscending) in sorts)
        {
            var member = GetMemberExpression(parameterExpression, propertyName);

            var sortBy = Expression.Lambda(member, parameterExpression);

            string methodName;
            if (isFirst)
            {
                methodName = isAscending ? "OrderBy" : "OrderByDescending";
                isFirst = false;
            }
            else
            {
                methodName = isAscending ? "ThenBy" : "ThenByDescending";
            }

            query = query.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable), methodName, [query.ElementType, sortBy.Body.Type], query.Expression, Expression.Quote(sortBy)));
        }

        return query;
    }
    #endregion


    #region Private methods
    private static MemberExpression GetMemberExpression(Expression parameterExpression, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException($"Parameter {nameof(propertyName)} cannot not be null, nor empty.", nameof(propertyName));
        }

        var propertySeparatorIndex = propertyName.IndexOf(".", StringComparison.Ordinal);
        if (propertySeparatorIndex >= 0)
        {
            var subParam = Expression.Property(parameterExpression, propertyName[..propertySeparatorIndex]);
            return GetMemberExpression(subParam, propertyName[(propertySeparatorIndex + 1)..]);
        }

        return Expression.Property(parameterExpression, propertyName);
    }

    private static async Task<QueryResult<T>> GetResultInternalAsync<T>(IQueryable<T> query, IQueryRequest<T> data, CancellationToken cancellationToken = default)
    {
        CheckPagingInfo(data, out var shouldComputeTotals);

        var queryable = query.Apply(data, applyFiltering: true, applyPagination: false, applySorting: false);

        var total = 0;
        if (shouldComputeTotals)
        {
            total = await QueryableAsyncResolver.Instance.CountAsync(queryable, cancellationToken);
        }

        queryable = queryable.Apply(data, applyFiltering: false, applyPagination: true, applySorting: true);

        IReadOnlyList<T> items = await QueryableAsyncResolver.Instance.ToListAsync(queryable, cancellationToken);

        if (!shouldComputeTotals)
        {
            total = items.Count;
        }

        var queryResult = new QueryResult<T>
        {
            Total = total,
            Items = items
        };

        return queryResult;
    }

    private static QueryResult<T> GetResultInternal<T>(IQueryable<T> query, IQueryRequest<T> data)
    {
        CheckPagingInfo(data, out var shouldComputeTotals);

        var queryable = query.Apply(data, applyFiltering: true, applyPagination: false, applySorting: false);

        var total = 0;
        if (shouldComputeTotals)
        {
            total = queryable.Count();
        }

        queryable = queryable.Apply(data, applyFiltering: false, applyPagination: true, applySorting: true);

        var items = queryable.ToList();

        if (!shouldComputeTotals)
        {
            total = items.Count;
        }

        var queryResult = new QueryResult<T>
        {
            Total = total,
            Items = items
        };

        return queryResult;
    }

    private static void CheckPagingInfo<T>(IQueryRequest<T> data, out bool shouldComputeTotals) => shouldComputeTotals = data.Skip > 0 || data.Take > 0;
    #endregion
}