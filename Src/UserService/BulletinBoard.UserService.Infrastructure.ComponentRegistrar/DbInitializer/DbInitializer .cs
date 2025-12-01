using BulletinBoard.Infrastructure.DataAccess.User.WriteContext;
using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace BulletinBoard.Infrastructure.ComponentRegistrar.DbInitializer;

/// <summary>
/// Проводит миграциб БД
/// </summary>
public class DbInitializer : IAsyncInitializer
{
    private readonly UserContext _userContext;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(
        UserContext userContext,
        ILogger<DbInitializer> logger
        )
    {
        _userContext = userContext;
        _logger = logger;
    }

    /// <summary>
    /// Проводит миграциб БД
    /// </summary>
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await _userContext.Database.MigrateAsync(cancellationToken);

    }

}