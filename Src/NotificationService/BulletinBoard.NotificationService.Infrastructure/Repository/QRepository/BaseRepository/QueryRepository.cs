using BulletinBoard.NotificationService.AppServices.Common.IRepository;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;


namespace BulletinBoard.NotificationService.Infrastructure.Repository.QRepository.BaseRepository;

public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class
{
    protected readonly NotificationDbContext _DbContext;

    protected DbSet<TEntity> _DbSet;

    public QueryRepository(NotificationDbContext dbContext)
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
