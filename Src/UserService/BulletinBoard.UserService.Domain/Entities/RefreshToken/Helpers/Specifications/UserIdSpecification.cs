using BulletinBoard.UserService.Domain.Helpers.SpecificationCore;
using System.Linq.Expressions;


namespace BulletinBoard.UserService.Domain.Entities.RefreshToken.Helpers.Specifications;

public class UserIdSpecification<T> : Specification<T>
    where T : IRefreshTokenFiltrationFieldsSet
{

    private readonly List<string> _id;

    public UserIdSpecification(List<string> id)
    {
        _id = id;
    }

    public UserIdSpecification(string id)
    {
        _id = new List<string>() { id };
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        return e => _id.Contains(e.UserId);
    }
}
