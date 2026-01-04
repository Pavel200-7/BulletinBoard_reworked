namespace BulletinBoard.UserService.AppServices.Common.IRepository;

/// <summary>
/// Базовый репозиторий записи.
/// </summary>
public interface ICommandRepository<TEntity> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
