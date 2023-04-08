using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using RestaurantAggregator.Auth.Data;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.Auth;

public class JwtAuthentication : IJwtAuthentication
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly SymmetricSecurityKey _key;
    private readonly TimeSpan _expirationTime;

    public JwtAuthentication(string secret)
    {
        _tokenHandler = new JwtSecurityTokenHandler();
        _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        _expirationTime = TimeSpan.FromHours(1);
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_expirationTime),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token, [NotNullWhen(true)] out SecurityToken? validatedToken)
    {
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            return validatedToken is JwtSecurityToken jwtSecurityToken
            && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
        catch (Exception)
        {
            validatedToken = null;
            return false;
        }
    }
}
