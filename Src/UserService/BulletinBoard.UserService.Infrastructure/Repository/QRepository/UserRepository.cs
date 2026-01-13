using BulletinBoard.UserService.AppServices.User.Helpers.Repository.UserRepository;
using BulletinBoard.UserService.Infrastructure.Repository.QRepository.BaseRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;


namespace BulletinBoard.UserService.Infrastructure.Repository.QRepository;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly IQueryRepository<IdentityUser> _repository;

    public UserRepository(
        ILogger<UserRepository> logger, 
        IQueryRepository<IdentityUser> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public Task<IdentityUser?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return _repository.GetQuery().FirstOrDefaultAsync(e => e.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<List<IdentityUser>> GetCursorPaginatedUsersData(
        CursorData cursorData,
        int limit,
        CancellationToken cancellationToken)
    {
        var query = _repository.GetQuery()
            .Where(e => e.UserName!.Contains(cursorData.Search));

        if (!cursorData.IsEmpty)
        {
            query = query.Where(p =>
                p.UserName!.CompareTo(cursorData.LastUserName) > 0 ||
                (p.UserName == cursorData.LastUserName && p.Id.CompareTo(cursorData.LastId) > 0));
        }

        return await query
            .OrderBy(p => p.UserName)
            .ThenBy(p => p.Id)
            .Take(limit + 1)
            .ToListAsync(cancellationToken);
    }
}
