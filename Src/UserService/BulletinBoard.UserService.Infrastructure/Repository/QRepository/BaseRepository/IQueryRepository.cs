using BulletinBoard.UserService.AppServices.Common.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace BulletinBoard.UserService.Infrastructure.Repository.QRepository.BaseRepository;

public class IQueryRepository<TEntity> : AppServices.Common.IRepository.IQueryRepository<TEntity> where TEntity : class
{
    private readonly UserDbContext _DbContext;
    private readonly DbSet<TEntity> _DbSet;

    public IQueryRepository(UserDbContext dbContext)
    {
        _DbContext = dbContext;
        _DbSet = _DbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> GetQuery()
    {
        return _DbSet.AsNoTracking();
    }

    public async Task<List<T>> GetWithSpecificationAsync<T>(
        Expression<Func<TEntity, bool>> expression, 
        Expression<Func<TEntity, T>> selector, 
        CancellationToken cancellationToken)
    {
        var query = GetQuery()
            .Where(expression)
            .Select(selector);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetWithSpecificationAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        return await GetWithSpecificationAsync(
            expression, 
            e => e, 
            cancellationToken);
    }

    public async Task<T?> GetFirstWithSpecificationAsync<T>(
        Expression<Func<TEntity, bool>> expression, 
        Expression<Func<TEntity, T>> selector, 
        CancellationToken cancellationToken)
    {
        var query = GetQuery()
            .Where(expression)
            .Select(selector);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity?> GetFirstWithSpecificationAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        return await GetFirstWithSpecificationAsync(
            expression,
            e => e,
            cancellationToken);
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
