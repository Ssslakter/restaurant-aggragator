using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Auth.Data;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAggregator.Auth.Services;

public interface IJwtAuthentication
{
    string GenerateToken(IEnumerable<Claim> claims, TimeSpan expireTime);
    TokenValidationParameters GenerateTokenValidationParameters();
    Task<Guid> GetUserIdFromTokenAsync(string token);
}

public class JwtAuthentication : IJwtAuthentication
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly SymmetricSecurityKey _key;
    private readonly AuthDbContext _context;
    private readonly ILogger<JwtAuthentication> _logger;

    public JwtAuthentication(IConfiguration configuration, AuthDbContext context, ILogger<JwtAuthentication> logger)
    {
        _context = context;
        _tokenHandler = new JwtSecurityTokenHandler();
#nullable disable
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]));
#nullable enable
        _logger = logger;
    }

    public string GenerateToken(IEnumerable<Claim> claims, TimeSpan expireTime)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = "RestaurantAggregator",
            Audience = "RestaurantAggregator",
            Expires = DateTime.UtcNow.Add(expireTime),
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

    public async Task<Guid> GetUserIdFromTokenAsync(string token)
    {
        if (await ValidateRefreshTokenAsync(token))
        {
            return Guid.Parse(_tokenHandler.ReadJwtToken(token).Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
        else
        {
            throw new AuthException("Refresh token is invalid");
        }
    }

    private async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
        try
        {
            var principal = _tokenHandler.ValidateToken(refreshToken,
            GenerateTokenValidationParameters(), out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            if (!await _context.RefreshTokens.AnyAsync(t => t.Token == refreshToken))
            {
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "Refresh token validation failed");
            return false;
        }
    }
}
