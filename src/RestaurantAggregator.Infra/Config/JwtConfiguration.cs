using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RestaurantAggregator.Infra.Config;
#nullable disable

public class JwtConfiguration
{
    public string Audience { get; set; } = "http://everyone";
    public string Issuer { get; set; } = "http://localhost";
    public string Secret { get; set; } = "MySecretVeryVeryLongKey123!!";
    public TimeSpan AccessTokenLifetime { get; set; } = TimeSpan.FromMinutes(20);
    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(7);

    public TokenValidationParameters GenerateTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            ClockSkew = TimeSpan.Zero,
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
        };
    }
}