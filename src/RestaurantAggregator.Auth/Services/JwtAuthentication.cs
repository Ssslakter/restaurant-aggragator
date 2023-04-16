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
    Task<string> GenerateRefreshTokenAsync(Guid userId);
    string GenerateAccessToken(IEnumerable<Claim> claims);
    Task<bool> ValidateRefreshTokenAsync(string token);
    Task RevokeAllUserRefreshTokensAsync(Guid userId);
}

public class JwtAuthentication : IJwtAuthentication
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly AuthDbContext _context;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly SymmetricSecurityKey _key;

    public JwtAuthentication(AuthDbContext context, IOptions<JwtConfiguration> jwtConfiguration)
    {
        _context = context;
        _tokenHandler = new JwtSecurityTokenHandler();
        _jwtConfiguration = jwtConfiguration.Value;
        _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.Secret));
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

    public async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var token = Convert.ToBase64String(randomNumber);
        await _context.RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = userId,
            Token = token,
            Expires = DateTime.UtcNow.Add(_jwtConfiguration.RefreshTokenLifetime)
        });
        await _context.SaveChangesAsync();
        return token;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            Expires = DateTime.UtcNow.Add(_jwtConfiguration.AccessTokenLifetime),
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }
}
