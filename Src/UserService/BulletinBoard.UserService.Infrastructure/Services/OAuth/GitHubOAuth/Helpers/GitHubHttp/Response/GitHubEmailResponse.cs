using System.Text.Json.Serialization;


namespace BulletinBoard.UserService.Infrastructure.Services.OAuth.GitHubOAuth.Helpers.GitHubHttp.Response;

public class GitHubEmailResponse
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("verified")]
    public bool Verified { get; set; }

    [JsonPropertyName("primary")]
    public bool Primary { get; set; }

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; } = string.Empty;
}
