using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantAggregator.Auth.Data;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Infra.Config;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using RestaurantAggregator.Auth.Data.Entities;

namespace RestaurantAggregator.Auth.Services;

public interface IJwtAuthentication
{
    Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId);
    AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
    Task<bool> ValidateRefreshTokenAsync(string token);
    Task RevokeAllUserRefreshTokensAsync(Guid userId);
}

public class JwtAuthentication : IJwtAuthentication
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly AuthDbContext _context;
    private readonly IOptions<JwtConfiguration> _jwtConfiguration;
    private readonly SymmetricSecurityKey _key;

    public JwtAuthentication(AuthDbContext context, IOptions<JwtConfiguration> jwtConfiguration)
    {
        _context = context;
        _tokenHandler = new JwtSecurityTokenHandler();
        _jwtConfiguration = jwtConfiguration;
        _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.Value.Secret));
    }
    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
        return await _context.RefreshTokens
        .AnyAsync(t => t.Token == refreshToken && t.Expires > DateTime.UtcNow);
    }

    public async Task RevokeAllUserRefreshTokensAsync(Guid userId)
    {
        await _context.RefreshTokens
            .Where(t => t.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId)
    {
        var currentToken = await _context.RefreshTokens
            .Where(t => t.UserId == userId && t.Expires > DateTime.UtcNow)
            .FirstOrDefaultAsync();
        if (currentToken != null)
        {
            return currentToken;
        }

        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var token = Convert.ToBase64String(randomNumber);
        var tokenEntity = new RefreshToken
        {
            UserId = userId,
            Token = token,
            Expires = DateTime.UtcNow.Add(_jwtConfiguration.Value.RefreshTokenLifetime)
        };
        await _context.RefreshTokens.AddAsync(tokenEntity);
        await _context.SaveChangesAsync();
        return tokenEntity;
    }

    public AccessToken GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtConfiguration.Value.Issuer,
            Audience = _jwtConfiguration.Value.Audience,
            Expires = DateTime.UtcNow.Add(_jwtConfiguration.Value.AccessTokenLifetime),
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = _tokenHandler.WriteToken(_tokenHandler.CreateToken(tokenDescriptor));
        return new AccessToken
        {
            Token = token,
            Expires = (DateTime)tokenDescriptor.Expires
        };
    }
}
