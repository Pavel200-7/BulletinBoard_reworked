using BulletinBoard.UserService.AppServices.Common.IRepository;
using Microsoft.EntityFrameworkCore;


namespace BulletinBoard.UserService.Infrastructure.Repository.CRepository.BaseRepository;

public class CommandRepository<TEntity> : ICommandRepository<TEntity> where TEntity : class
{
    protected readonly DbContext _DbContext;

    protected DbSet<TEntity> _DbSet;

    public CommandRepository(UserDbContext dbContext)
    {
        _DbContext = dbContext;
        _DbSet = _DbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _DbSet.FindAsync(id, cancellationToken);
        return entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _DbSet.AddAsync(entity);
        await _DbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _DbSet.Update(entity);
        await _DbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _DbSet.FindAsync(id);
        if (entity != null)
        {
            _DbSet.Remove(entity);
            await _DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
