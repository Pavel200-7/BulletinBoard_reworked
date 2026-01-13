using BulletinBoard.UserService.AppServices.User.Admin.Queries.GetUsersData;
using BulletinBoard.UserService.AppServices.User.Helpers.Repository.UserRepository;
using BulletinBoard.UserService.Domain.Entities.RefreshToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.AdminTests.QueriesTests.GetUsersDataTests;

public class GetUsersDataQueryHandlerTests
{
    private Mock<ILogger<GetUsersDataQueryHandler>> _logger;
    private Mock<IUserRepository> _qRepository;
    private GetUsersDataQueryHandler _handler;
    private CancellationToken _cancellationToken;

    public GetUsersDataQueryHandlerTests()
    {
        _logger = new Mock<ILogger<GetUsersDataQueryHandler>>();
        _qRepository = new Mock<IUserRepository>();
        _handler = new GetUsersDataQueryHandler(_logger.Object, _qRepository.Object);
        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task PutNotEmptyCursorDataWhenHaveStringForDeserialize()
    {
        // Arrange
        var query = CreateQuery(CreateSerializedCursor());

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        // Assert
        _qRepository.Verify(r => r.GetCursorPaginatedUsersData(
            It.Is<CursorData>(d => d.IsEmpty == false),
            query.Limit,
            _cancellationToken), Times.Once);
    }


    [Fact]
    public async Task PutEmptyCursorDataWhenNotHaveStringForDeserialize()
    {
        // Arrange
        var query = CreateQuery("");

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        // Assert
        _qRepository.Verify(r => r.GetCursorPaginatedUsersData(
            It.Is<CursorData>(d => d.IsEmpty == true),
            query.Limit,
            _cancellationToken), Times.Once);
    }

    private void SetupMock()
    {
    }

    private GetUsersDataQuery CreateQuery(string cursor)
    {
        return new GetUsersDataQuery()
        {
            Search = GetSearch(),
            Cursor = cursor,
            Limit = 20
        };
    }

    private string CreateSerializedCursor()
    {
        return $"SomeName|SomeId|{GetSearch()}";
    }

    private string GetSearch()
    {
        return "SomeSearch";
    }
}
