using BulletinBoard.Infrastructure.DataAccess.User.WriteContext;
using BulletinBoard.UserService.AppServices.Common.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;


namespace BulletinBoard.UserService.Infrastructure.DataAccess.Common.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(UserContext context)
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
