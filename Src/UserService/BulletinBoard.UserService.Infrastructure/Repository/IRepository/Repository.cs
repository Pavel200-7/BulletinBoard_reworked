using Microsoft.EntityFrameworkCore;


namespace BulletinBoard.UserService.Infrastructure.Repository.IRepository;

public class Repository<TEntity, TContext> : IRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
{
    protected readonly TContext DbContext;

    protected DbSet<TEntity> DbSet;

    public Repository(TContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }
    public IQueryable<TEntity> GetAll()
    {
        return DbSet;
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
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
        var entity = await GetByIdAsync(id);

        if (entity != null)
        {
            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
