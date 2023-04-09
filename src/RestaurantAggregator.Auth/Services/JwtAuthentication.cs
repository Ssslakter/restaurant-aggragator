using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RestaurantAggregator.Auth.Services;

public interface IJwtAuthentication
{
    string GenerateToken(IEnumerable<Claim> claims);
    TokenValidationParameters GenerateTokenValidationParameters();
}

public class JwtAuthentication : IJwtAuthentication
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly SymmetricSecurityKey _key;
    private readonly TimeSpan _expirationTime;

    public JwtAuthentication(IConfiguration configuration)
    {
        _tokenHandler = new JwtSecurityTokenHandler();
#nullable disable
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]));
#nullable enable
        _expirationTime = TimeSpan.FromHours(1);
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = "RestaurantAggregator",
            Audience = "RestaurantAggregator",
            Expires = DateTime.UtcNow.Add(_expirationTime),
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public TokenValidationParameters GenerateTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "RestaurantAggregator",
            ValidAudience = "RestaurantAggregator",
            ClockSkew = TimeSpan.Zero,
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
        };
    }
}
