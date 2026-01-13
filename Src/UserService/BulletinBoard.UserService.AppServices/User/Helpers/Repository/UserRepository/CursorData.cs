namespace BulletinBoard.UserService.AppServices.User.Helpers.Repository.UserRepository;

public class CursorData
{
    public string LastId { get; init; }
    public string LastUserName { get; init; }
    public string Search { get; init; }
    public bool IsEmpty { get; init; }

    public CursorData(string search)
    {
        Search = search;
        IsEmpty = true;
    }

    private CursorData(string lastUserName, string lastId, string search)
    {
        LastUserName = lastUserName;
        LastId = lastId;
        Search = search;
        IsEmpty = false;
    }

    public static CursorData Deserialize(string stringForDeserialize)
    {
        try
        {
            string[] parts = stringForDeserialize.Split("|");
            string lastUserName = parts[0];
            string lastId = parts[1];
            string search = parts[2];

            return new CursorData(lastUserName, lastId, search);
        }
        catch 
        {
            throw new ArgumentException("Невозможно десериализовать данные курсора");
        }
    }

    public string Serialize()
    {
        return $"{LastUserName}|{LastId}|{Search}"; ;
    }

    public static string Serialize(string lastUserName, string lastId, string search)
    {
        return $"{lastUserName}|{lastId}|{search}"; ;
    }
}
