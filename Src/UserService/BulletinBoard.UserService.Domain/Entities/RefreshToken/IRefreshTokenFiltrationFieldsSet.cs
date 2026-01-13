using BulletinBoard.UserService.Domain.Entities.Base;


namespace BulletinBoard.UserService.Domain.Entities.RefreshToken;

public interface IRefreshTokenFiltrationFieldsSet : IBaseEntity
{
    public string Token { get; set; }
    public string UserId { get; set; }
}
