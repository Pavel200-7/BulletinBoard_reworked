using BulletinBoard.NotificationService.AppServices.Common.IRepository;
using Microsoft.EntityFrameworkCore;


namespace BulletinBoard.NotificationService.Infrastructure.Repository.CRepository;

public class CommandRepository<TEntity> : ICommandRepository<TEntity> where TEntity : class
{
    protected readonly DbContext DbContext;

    protected DbSet<TEntity> DbSet;

    public CommandRepository(NotificationDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
