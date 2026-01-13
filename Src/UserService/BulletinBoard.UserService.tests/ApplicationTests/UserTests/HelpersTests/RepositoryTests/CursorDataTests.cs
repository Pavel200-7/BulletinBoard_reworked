using BulletinBoard.UserService.AppServices.User.Helpers.Repository.UserRepository;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.HelpersTests.RepositoryTests;

public class CursorDataTests
{
    [Fact]
    public void CreateEmptyWhenWithOnlySearchParam()
    {
        // Arrange
        string expectedSearch = GetSearch();

        // Act 
        CursorData cursorData = new CursorData(expectedSearch);

        // Assert
        Assert.True(cursorData.IsEmpty);
        Assert.Equal(expectedSearch, cursorData.Search);
    }

    [Fact]
    public void CreateSerializedStringFromParams()
    {
        // Arrange 
        string lastUserName = GetName();
        string lastId = GetId();
        string search = GetSearch();
        string expectedSerializedString = GetSerializedString();

        // Act 
        string SerializedString = CursorData.Serialize(lastUserName, lastId, search);

        // Assert
        Assert.Equal(expectedSerializedString, SerializedString);
    }

    [Fact]
    public void CreateWithDatsWhenCreatedWithSerializedString()
    {
        // Arrange 
        string serializedString = GetSerializedString();
        string expectedLastUserName = GetName();
        string expectedLastId = GetId();
        string expectedSearch = GetSearch();

        // Act 
        CursorData cursorData = CursorData.Deserialize(serializedString);

        // Assert
        Assert.False(cursorData.IsEmpty);
        Assert.Equal(expectedLastUserName, cursorData.LastUserName);
        Assert.Equal(expectedLastId, cursorData.LastId);
        Assert.Equal(expectedSearch, cursorData.Search);

    }

    [Fact]
    public void CreateSerializedStringFromObject()
    {
        // Arrange 
        string expectedSerializedString = GetSerializedString();
        CursorData cursorData = CursorData.Deserialize(expectedSerializedString);

        // Act 
        string serializedString = cursorData.Serialize();

        // Assert
        Assert.Equal(expectedSerializedString, serializedString);
    }

    private string GetName()
    {
        return "SomeName";
    }

    private string GetId()
    {
        return "SomeId";
    }

    private string GetSearch()
    {
        return "SomeSearch";
    }

    private string GetSerializedString()
    {
        string lastUserName = GetName();
        string lastId = GetId();
        string search = GetSearch();
        return $"{lastUserName}|{lastId}|{search}";
    }
}
