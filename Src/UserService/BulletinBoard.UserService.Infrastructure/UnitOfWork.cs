using BulletinBoard.UserService.AppServices.Common.IRepository;
using Microsoft.EntityFrameworkCore.Storage;


namespace BulletinBoard.UserService.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(UserDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose() => _context?.Dispose();
}