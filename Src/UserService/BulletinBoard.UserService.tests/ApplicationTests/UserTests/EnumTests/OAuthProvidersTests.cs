using BulletinBoard.NotificationService.AppServices.User.Enum;


namespace BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.EnumTests;

public class OAuthProvidersTests
{
    [Theory]
    [InlineData("unknownProvider1")]
    [InlineData("unknownProvider2")]
    public void IsProviderWhenUnknown(string providerCandidate)
    {
        // Act 
        bool IsProvider = OAuthProviders.IsProvider(providerCandidate);

        // Assert
        Assert.False(IsProvider);
    }

    [Theory]
    [InlineData(OAuthProviders.GitHub)]
    public void IsProviderWhenExists(string provider)
    {
        // Act 
        bool IsProvider = OAuthProviders.IsProvider(provider);

        // Assert
        Assert.True(IsProvider);
    }
}
