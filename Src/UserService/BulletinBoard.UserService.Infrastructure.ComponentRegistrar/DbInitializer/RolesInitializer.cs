using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Extensions.Hosting.AsyncInitialization;
using BulletinBoard.UserService.AppServices.User.Enum;

namespace BulletinBoard.UserService.Infrastructure.Initializers;

/// <summary>
/// Инициализатор ролей в системе
/// </summary>
public class RolesInitializer : IAsyncInitializer
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<RolesInitializer> _logger;

    public RolesInitializer(
        RoleManager<IdentityRole> roleManager,
        ILogger<RolesInitializer> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    /// <summary>
    /// Инициализирует роли в системе
    /// </summary>
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await InitializeRoleAsync(Roles.Admin);
        await InitializeRoleAsync(Roles.User);
    }

    /// <summary>
    /// Создает роль, если она не существует
    /// </summary>
    /// <param name="roleName">Название роли</param>
    private async Task InitializeRoleAsync(string roleName)
    {
        try
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                _logger.LogInformation("Создание роли {RoleName}", roleName);

                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    _logger.LogInformation("Роль {RoleName} успешно создана", roleName);
                }
                else
                {
                    _logger.LogError(
                        "Не удалось создать роль {RoleName}. Ошибки: {Errors}",
                        roleName,
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                _logger.LogDebug("Роль {RoleName} уже существует", roleName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании роли {RoleName}", roleName);
            throw;
        }
    }
}