using BulletinBoard.UserService.Domain.Helpers.SpecificationCore.Helpers.LogicalOperations;
using System.Linq.Expressions;


namespace BulletinBoard.UserService.Domain.Helpers.SpecificationCore;

/// <summary>
/// Базовый класс спецификаций.
/// </summary>
public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Проверяет, соответствует ли сущность критериям спецификации.
    /// </summary>
    public virtual bool IsSatisfied(T entity)
    {
        Func<T, bool> predicate = ToExpression().Compile();
        return predicate(entity);
    }

    /// <summary>
    /// Операция конъюнкции, логического "И" над двумя спецификациями.
    /// Над текущей и той, которая передана в качестве аргумента.
    /// из двух спецификаций генерируем новую.
    /// </summary>
    public Specification<T> And(Specification<T> specification)
        => new AndSpecification<T>(this, specification);

    /// <summary>
    /// Операция дизъюнкции, логического "ИЛИ" над двумя спецификациями. 
    /// Над текущей и той, которая передана в качестве аргумента.
    /// из двух спецификаций генерируем новую.
    /// </summary>
    public Specification<T> Or(Specification<T> specification)
        => new OrSpecification<T>(this, specification);

    /// <summary>
    /// Операция логического отрицания.
    /// генерируем новую спецификацию, которая является отрицанием исходной.
    /// </summary>
    public Specification<T> Not()
        => new NotSpecification<T>(this);

    /// <summary>
    /// Оператор true для спецификации.
    /// </summary>
    public static bool operator true(Specification<T> specification) 
        => true;

    /// /// <summary>
    /// Оператор false для спецификации.
    /// </summary>
    public static bool operator false(Specification<T> specification) 
        => false;

    /// <summary>
    /// Оператор логического "И".
    /// </summary>
    public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        => left.Or(right);

    /// <summary>
    /// Оператор логического "ИЛИ".
    /// </summary>
    public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        => left.And(right);

    /// <summary>
    /// Оператор логического отрицания.
    /// </summary>
    public static Specification<T> operator !(Specification<T> specification)
        => specification.Not();
}
