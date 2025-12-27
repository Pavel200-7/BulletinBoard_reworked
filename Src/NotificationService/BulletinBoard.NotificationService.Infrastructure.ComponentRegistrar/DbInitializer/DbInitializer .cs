using BulletinBoard.NotificationService.Infrastructure;
using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.Infrastructure.ComponentRegistrar.DbInitializer;

/// <summary>
/// Проводит миграциб БД
/// </summary>
public class DbInitializer : IAsyncInitializer
{
    private readonly NotificationDbContext _context;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(
        NotificationDbContext context,
        ILogger<DbInitializer> logger
        )
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Проводит миграциб БД
    /// </summary>
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await _context.Database.MigrateAsync(cancellationToken);
    }

}