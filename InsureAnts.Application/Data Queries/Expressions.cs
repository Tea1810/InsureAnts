using System.Linq.Expressions;

namespace InsureAnts.Application.Data_Queries;

public static class Expressions
{
    private class ExpressionParametersRewrite : ExpressionVisitor
    {
        private readonly IDictionary<ParameterExpression, ParameterExpression> _parameterReplacements;

        private ExpressionParametersRewrite(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
        {
            _parameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
            for (var i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
            {
                _parameterReplacements.Add(fromParameters[i], toParameters[i]);
            }
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameterReplacements.TryGetValue(node, out var replacement))
            {
                node = replacement;
            }

            return base.VisitParameter(node);
        }

        public static Expression RewriteParameters(LambdaExpression expression, LambdaExpression withExpression)
        {
            return new ExpressionParametersRewrite(expression.Parameters, withExpression.Parameters).Visit(expression.Body);
        }
    }

    private static Expression<Func<T, bool>> OrElseInternal<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
    {
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expression1.Body, ExpressionParametersRewrite.RewriteParameters(expression2, expression1)), expression1.Parameters);
    }

    public static Expression<Func<T, bool>> OrElse<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2, params Expression<Func<T, bool>>[] expressions)
    {
        var resultExpression = OrElseInternal(expression1, expression2);
        foreach (var expression in expressions)
        {
            resultExpression = OrElseInternal(resultExpression, expression);
        }
        return resultExpression;
    }

    private static Expression<Func<T, bool>> AndAlsoInternal<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
    {
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expression1.Body, ExpressionParametersRewrite.RewriteParameters(expression2, expression1)), expression1.Parameters);
    }

    public static Expression<Func<T, bool>> AndAlso<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2, params Expression<Func<T, bool>>[] expressions)
    {
        var resultExpression = AndAlsoInternal(expression1, expression2);
        foreach (var expression in expressions)
        {
            resultExpression = AndAlsoInternal(resultExpression, expression);
        }
        return resultExpression;
    }

    public static Func<TSource, TResult> DirectCast<TSource, TResult>()
    {
        var parameter = Expression.Parameter(typeof(TSource));

        var dynamicMethod = Expression.Lambda<Func<TSource, TResult>>(Expression.Convert(parameter, typeof(TResult)), parameter);

        return dynamicMethod.Compile();
    }
}
