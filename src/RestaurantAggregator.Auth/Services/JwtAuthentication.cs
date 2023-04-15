using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Auth.Data;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Core.Config;

namespace RestaurantAggregator.Auth.Services;

public interface IJwtAuthentication
{
    string GenerateToken(IEnumerable<Claim> claims, bool isRefreshToken = false);
    TokenValidationParameters GenerateTokenValidationParameters();
    Task<Guid> GetUserIdFromTokenAsync(string token);
}

public class JwtAuthentication : IJwtAuthentication
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly AuthDbContext _context;
    private readonly ILogger<JwtAuthentication> _logger;
    private readonly IJwtConfiguration _jwtConfiguration;
    private readonly SymmetricSecurityKey _key;

    public JwtAuthentication(AuthDbContext context, ILogger<JwtAuthentication> logger, IJwtConfiguration jwtConfiguration)
    {
        _context = context;
        _tokenHandler = new JwtSecurityTokenHandler();
        _jwtConfiguration = jwtConfiguration;
        _logger = logger;
        _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.Secret));
    }

    public string GenerateToken(IEnumerable<Claim> claims, bool isRefreshToken = false)
    {
        var expireTime = isRefreshToken ? _jwtConfiguration.RefreshTokenLifetime : _jwtConfiguration.AccessTokenLifetime;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
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
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidAudience = _jwtConfiguration.Audience,
            ClockSkew = TimeSpan.Zero,
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
        };
    }

    public async Task<Guid> GetUserIdFromTokenAsync(string token)
    {
        if (await ValidateRefreshTokenAsync(token))
        {
#nullable disable
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault();
            return Guid.Parse(claim.Value);
#nullable enable
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
