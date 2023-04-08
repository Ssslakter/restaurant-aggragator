using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Data.Entities;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Auth.Services;

public interface IAuthenticationService
{
    User Authenticate(string email, string password);
    string GenerateJwtToken(User user);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly AuthDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtAuthentication _jwtAuthentication;

    public AuthenticationService(AuthDbContext context, IPasswordHasher<User> passwordHasher, IJwtAuthentication jwtAuthentication)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtAuthentication = jwtAuthentication;
    }

    public User Authenticate(string email, string password)
    {
        var user = _context.Users.Include(x => x.Roles).SingleOrDefault(x => x.Email == email);

        if (user == null)
            throw new NotFoundInDbException("User not found");

        if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Success)
            throw new AuthException("Email or password is incorrect");

        return user;
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }

        return _jwtAuthentication.GenerateToken(claims);
    }
}
