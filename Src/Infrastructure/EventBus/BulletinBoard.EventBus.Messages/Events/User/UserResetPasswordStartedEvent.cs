namespace BulletinBoard.EventBus.Messages.Events.User;

public record UserResetPasswordStartedEvent
{
    public string Id { get; set; }
    public string Token { get; set; }

    public UserResetPasswordStartedEvent(string id, string token)
    {
        Id = id;
        Token = token;
    }
}
