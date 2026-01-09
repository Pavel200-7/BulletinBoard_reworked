using BulletinBoard.UserService.AppServices.Common.IRepository;
using Microsoft.EntityFrameworkCore;


namespace BulletinBoard.UserService.Infrastructure.Repository.QRepository.BaseRepository;

public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class
{
    private readonly UserDbContext _DbContext;
    private readonly DbSet<TEntity> _DbSet;

    public QueryRepository(UserDbContext dbContext)
    {
        _DbContext = dbContext;
        _DbSet = _DbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll()
    {
        return _DbSet.AsNoTracking();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _DbSet.FindAsync(id, cancellationToken);
        if (entity != null)
        {
            _DbContext.Entry(entity).State = EntityState.Detached; 
        }
        return entity;
    }
}
