using System.Linq.Expressions;
using System.Linq;
using InsureAnts.Application.Features.Abstractions;

namespace InsureAnts.Application.Data_Queries;

public abstract class AbstractQueryRequest<T> : IQueryRequest<T>, IQuery<QueryResult<T>>
{
    #region Proerties
    private int? _skip;

    public int? Skip
    {
        get => _skip;
        set => _skip = value > 0 ? value : null;
    }

    private int? _take;

    public int? Take
    {
        get => _take;
        set => _take = value > 0 ? value : null;
    }

    public Dictionary<string, bool> Sort { get; set; }
    #endregion


    protected AbstractQueryRequest()
    {
        Sort = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
    }


    #region Public methods
    public virtual IQueryable<T> ApplyFilter(IQueryable<T> source)
    {
        var query = source;

        var filterExpression = TryGetFilterExpression();
        if (filterExpression != null)
        {
            query = query.Where(filterExpression);
        }

        return query;
    }

    public IQueryable<T> ApplySkipTake(IQueryable<T> source)
    {
        var queryable = source;

        if (Skip > 0)
        {
            queryable = queryable.Skip(Skip.Value);
        }

        if (Take > 0)
        {
            queryable = queryable.Take(Take.Value);
        }

        return queryable;
    }
    #endregion


    #region Abstract methods
    public IQueryable<T> ApplySort(IQueryable<T> source)
    {
        var query = source;

        query = Sort is { Count: > 0 } ? ApplySort(query, Sort) : ApplyDefaultSort(query);
        return query;
    }

    protected virtual IQueryable<T> ApplySort(IQueryable<T> source, Dictionary<string, bool> sort) => source.ApplySort(sort);

    protected virtual IQueryable<T> ApplyDefaultSort(IQueryable<T> source) => source;

    protected virtual Expression<Func<T, bool>>? TryGetFilterExpression() => null;
    #endregion
}