using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;

public class JWTProvider : IJWTProvider
{
    private readonly UserManager<IdentityUser> _userManager;
    private JwtSecurityTokenHandler _tokenHandler;
    private JwtSettings _jwtSettings;

    public JWTProvider(UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _tokenHandler = new JwtSecurityTokenHandler();
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<TokenData> GenerateToken(string userId, CancellationToken cancellationToken)
    {
        var claims = await GetClaims(userId, cancellationToken);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(_jwtSettings.ExpiresIn), 
            signingCredentials: creds
        );

        var tokenString = _tokenHandler.WriteToken(token);
        return new TokenData()
        {
            TokenType = "Bearer",
            ExpiresIn = _jwtSettings.ExpiresIn,
            AccessToken = tokenString
        };
    }

    private async Task<Claim[]> GetClaims(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("Пользователь с данным id не обнаружен.");
        }
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Sid, user.Id));
        claims.Add(new Claim(ClaimTypes.Email, user.Email!));
        roles.ToList()
            .ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));

        return claims.ToArray();
    }
}
