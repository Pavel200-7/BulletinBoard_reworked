using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter.DTO;


namespace BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter;

/// <summary>
/// Интерфейс, задающий стандарт операций авторизации для внешних провайдеров.
/// </summary>
public interface IAuthServiceAdapter
{
    public Task<bool> RegisterAsync(UserCreateDto userDto, CancellationToken cancellationToken);
    public Task<UserDto?> GetByLoginAsync(string login, CancellationToken cancellationToken);
    public Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    public Task<bool> AddRoleByEmailAsync(string email, string role, CancellationToken cancellationToken);
}
