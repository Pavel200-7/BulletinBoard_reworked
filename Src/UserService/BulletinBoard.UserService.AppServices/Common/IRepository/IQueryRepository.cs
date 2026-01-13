using System.Linq.Expressions;

namespace BulletinBoard.UserService.AppServices.Common.IRepository;

/// <summary>
/// Базовый репозиторий чтения.
/// </summary>
public interface IQueryRepository<TEntity> where TEntity : class
{
    public IQueryable<TEntity> GetQuery();
    public Task<List<T>> GetWithSpecificationAsync<T>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, T>> selector,
        CancellationToken cancellationToken);
    public Task<List<TEntity>> GetWithSpecificationAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken);
    public Task<T?> GetFirstWithSpecificationAsync<T>(
        Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, T>> selector,
        CancellationToken cancellationToken);
    public Task<TEntity?> GetFirstWithSpecificationAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken);
    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}

