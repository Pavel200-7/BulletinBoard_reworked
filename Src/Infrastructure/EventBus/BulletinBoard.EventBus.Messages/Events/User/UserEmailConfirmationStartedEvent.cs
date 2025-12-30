namespace BulletinBoard.EventBus.Messages.Events.User;

public record UserEmailConfirmationStartedEvent
{
    public string Id { get; set; }
    public string Token { get; set; }

    public UserEmailConfirmationStartedEvent(string id, string token)
    {
        Id = id;
        Token = token;
    }
}
