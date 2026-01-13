using BulletinBoard.UserService.Domain.Helpers.SpecificationCore;
using System.Linq.Expressions;

namespace BulletinBoard.UserService.Domain.Entities.RefreshToken.Helpers.Specifications;

public class TokenStringSpecification<T> : Specification<T>
    where T : IRefreshTokenFiltrationFieldsSet
{
    private readonly string _tokenString;

    public TokenStringSpecification(string tokenString)
    {
        _tokenString = tokenString;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        return e => e.Token == _tokenString;
    }
}
