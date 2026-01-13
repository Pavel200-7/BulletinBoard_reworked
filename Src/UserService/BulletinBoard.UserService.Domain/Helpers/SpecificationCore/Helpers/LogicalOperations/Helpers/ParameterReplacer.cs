using System.Linq.Expressions;


namespace BulletinBoard.UserService.Domain.Helpers.SpecificationCore.Helpers.LogicalOperations.Helpers;

/// <summary>
/// Класс для замены параметров в выражениях LINQ.
/// Используется, когда нужно объединить или изменить выражения, чтобы параметры совпадали.
/// Например, при комбинировании двух выражений, у которых разные параметры.
/// </summary>
public class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _parameter;

    /// <summary>
    /// Конструктор принимает параметр, на который нужно заменить все параметры в выражении.
    /// </summary>
    /// <param name="parameter">Параметр, которым нужно заменить все входные параметры.</param>
    public ParameterReplacer(ParameterExpression parameter)
    {
        _parameter = parameter;
    }

    /// <summary>
    /// Переопределение метода VisitParameter, который вызывается для каждого узла типа ParameterExpression.
    /// Здесь мы заменяем текущий параметр на заданный в конструкторе.
    /// </summary>
    /// <param name="node">Исходный параметр в выражении.</param>
    /// <returns>Заменённый параметр.</returns>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return base.VisitParameter(_parameter);
    }
}
