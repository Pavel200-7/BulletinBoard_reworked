using System.Text.Json.Serialization;

namespace BulletinBoard.UserService.Infrastructure.Services.OAuth.GitHubOAuth.Helpers.GitHubHttp.Response;

public class GitHubAccessTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;

    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;

    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;

    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; set; } = string.Empty;

    [JsonPropertyName("error_uri")]
    public string ErrorUri { get; set; } = string.Empty;
}
