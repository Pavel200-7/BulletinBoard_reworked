using BulletinBoard.NotificationService.AppServices.Common.Configurations;
using BulletinBoard.UserService.Infrastructure.Services.OAuth.GitHubOAuth.Helpers.GitHubHttp.Response;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;


namespace BulletinBoard.NotificationService.Infrastructure.Services.OAuth.GitHubOAuth.Helpers.GitHubHttp;


public class GitHubHttpService : IGitHubHttpService
{
    private readonly ILogger<GitHubHttpService> _logger;
    private readonly HttpClient _httpClient;
    private readonly GitHubOAuthSettings _settings;

    public GitHubHttpService(
        ILogger<GitHubHttpService> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<GitHubOAuthSettings> settings)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _settings = settings.Value;
    }

    public async Task<string> GetAccessToken(string code, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начало получения GitHub access токена.");
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", _settings.ClientId),
            new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("redirect_uri", _settings.RedirectURI)
        });
        var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token")
        {
            Content = content
        };
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Получение GitHub access токена провалилось: {StatusCode} - {Content}.",
                response.StatusCode, errorContent);
            throw new HttpRequestException($"Получение GitHub access токена провалилось: {response.StatusCode}.");
        }

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var tokenResponse = JsonSerializer.Deserialize<GitHubAccessTokenResponse>(responseJson);
        ValidateResponse(tokenResponse);

        _logger.LogInformation("Access токен GitHub был успешно получен.");
        return tokenResponse!.AccessToken;
    }

    private void ValidateResponse(GitHubAccessTokenResponse? response)
    {
        if (string.IsNullOrEmpty(response?.AccessToken))
        {
            _logger.LogError("GitHub вернул пустой access токен.");
            throw new InvalidOperationException("GitHub вернул пустой access токен.");
        }

        if (!string.IsNullOrEmpty(response.Error))
        {
            _logger.LogError("Ошибка получения GitHub access токена: {Error} - {Description}.",
                response.Error, response.ErrorDescription);
            throw new InvalidOperationException($"Ошибка получения GitHub access токена: {response.Error}.");
        }
    }

    public async Task<string> GetUserEmail(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.UserAgent.Add(new ProductInfoHeaderValue("BulletinBoard", "1.0"));
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var emailsJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var emails = JsonSerializer.Deserialize<List<GitHubEmailResponse>>(emailsJson);

        var primaryEmail = emails?.FirstOrDefault(e => e.Primary && e.Verified)?.Email
                        ?? emails?.FirstOrDefault(e => e.Primary)?.Email
                        ?? emails?.FirstOrDefault(e => e.Verified)?.Email
                        ?? emails?.FirstOrDefault()?.Email;

        return primaryEmail!;
    }
}
