using BulletinBoard.UserService.Domain.Helpers.SpecificationCore.Helpers.LogicalOperations.Helpers;
using System.Linq.Expressions;


namespace BulletinBoard.UserService.Domain.Helpers.SpecificationCore.Helpers.LogicalOperations;

/// <summary>
/// Спецификация для логического И между двумя условиями.
/// </summary>
public class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    /// <inheritdoc/>
    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    /// <inheritdoc/>
    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        ParameterExpression paramExpression = Expression.Parameter(typeof(T));
        BinaryExpression expressionBody = Expression.OrElse(leftExpression, rightExpression);

        expressionBody = (BinaryExpression)new ParameterReplacer(paramExpression).Visit(expressionBody);

        return Expression.Lambda<Func<T, bool>>(expressionBody, paramExpression);
    }
}
