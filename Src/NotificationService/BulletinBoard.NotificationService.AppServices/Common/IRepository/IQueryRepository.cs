namespace BulletinBoard.NotificationService.AppServices.Common.IRepository;

/// <summary>
/// Базовый репозиторий чтения.
/// </summary>
public interface IQueryRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
