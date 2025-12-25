using BulletinBoard.UserService.AppServices.Common.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.UserService.Infrastructure.Repository.QRepository.BaseRepository;

public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class
{
    protected readonly UserDbContext DbContext;

    protected DbSet<TEntity> DbSet;

    public QueryRepository(UserDbContext dbContext)
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
}
