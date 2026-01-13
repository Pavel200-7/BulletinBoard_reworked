using System.Linq.Expressions;


namespace BulletinBoard.UserService.Domain.Helpers.SpecificationCore.Helpers.LogicalOperations;

/// <summary>
/// Спецификация для отрицания (NOT) условия.
/// Оборачивает другую спецификацию и возвращает её логическое отрицание.
/// </summary>
public class NotSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;

    /// <inheritdoc/>
    public NotSpecification(Specification<T> left)
    {
        _left = left;
    }

    /// <inheritdoc/>
    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        
        UnaryExpression NotBody = Expression.Not(leftExpression.Body);
        ParameterExpression param = leftExpression.Parameters[0];

        return Expression.Lambda<Func<T, bool>>(NotBody, param);
    }
}
