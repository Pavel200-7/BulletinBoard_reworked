using BulletinBoard.UserService.AppServices.Common.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;

public class JWTProvider : IJWTProvider
{
    private JwtSecurityTokenHandler _tokenHandler;
    private JwtSettings _jwtSettings;

    public JWTProvider(IOptions<JwtSettings> jwtSettings)
    {
        _tokenHandler = new JwtSecurityTokenHandler();
        _jwtSettings = jwtSettings.Value;
    }

    public TokenData GenerateToken(ClaimsData claimsData)
    {
        var claims = GetClaims(claimsData);
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

    private Claim[] GetClaims(ClaimsData claimsData)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Sid, claimsData.UserId));
        claims.Add(new Claim(ClaimTypes.Email, claimsData.Email));
        claimsData.Roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));

        return claims.ToArray();
    }
}
