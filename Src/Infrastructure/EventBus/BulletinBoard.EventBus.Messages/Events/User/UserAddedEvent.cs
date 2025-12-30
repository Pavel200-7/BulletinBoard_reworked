namespace BulletinBoard.EventBus.Messages.Events.User;

public record UserAddedEvent
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
