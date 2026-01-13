using BulletinBoard.UserService.Domain.Entities.Base;


namespace BulletinBoard.UserService.Domain.Entities.RefreshToken;

public class RefreshToken : BaseEntity, IRefreshTokenFiltrationFieldsSet
{
    public string Token { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool Used { get; set; }
    public string UserId { get; set; }

    public RefreshToken(string userId, DateTime expiryDate)
    {
        UserId = userId;
        ExpiryDate = expiryDate;

        Token = Guid.NewGuid().ToString();
        CreationDate = DateTime.UtcNow;
        Used = false;
    }
}
