namespace BulletinBoard.NotificationService.AppServices.Common.IRepository;

public interface IQueryRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(Guid id);
}
