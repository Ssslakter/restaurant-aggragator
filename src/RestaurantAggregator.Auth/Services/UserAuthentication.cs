using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Data.Entities;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Auth.Data;
using RestaurantAggregator.Auth.Data.DTO;
using RestaurantAggregator.Auth.Utils;

namespace RestaurantAggregator.Auth.Services;

public interface IUserAuthentication
{
    Task<User> Login(string email, string password);

    Task<User> Register(RegistrationModel registrationModel);
    string GenerateJwtToken(User user);
    Task Logout(string token, Guid userId);
}

public class UserAuthentication : IUserAuthentication
{
    private readonly AuthDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtAuthentication _jwtAuthentication;
    private readonly ITokenStorage _tokenStorage;
    private readonly ILogger<UserAuthentication> _logger;

    public UserAuthentication(AuthDbContext context, IPasswordHasher<User> passwordHasher,
        IJwtAuthentication jwtAuthentication, ILogger<UserAuthentication> logger, ITokenStorage tokenStorage)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtAuthentication = jwtAuthentication;
        _logger = logger;
        _tokenStorage = tokenStorage;
    }

    public async Task<User> Login(string email, string password)
    {
        var user = await _context.Users.Include(x => x.Roles).SingleOrDefaultAsync(x => x.Email == email);

        if (user == null)
            throw new NotFoundInDbException("User not found");

        if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Success)
            throw new AuthException("Password is incorrect");

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
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        return _jwtAuthentication.GenerateToken(claims);
    }

    public async Task<User> Register(RegistrationModel registrationModel)
    {
        var user = new User
        {
            Email = registrationModel.Email,
            Name = registrationModel.Name,
            FullName = $"{registrationModel.Surname} {registrationModel.Name} {registrationModel.MiddleName}",
            Phone = registrationModel.Phone,
            Address = registrationModel.Address,
            Roles = new List<UserRole>()
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, registrationModel.Password);
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new AuthException("User with this email already exists");
        }

        return user;
    }

    public async Task Logout(string token, Guid userId)
    {
        await _tokenStorage.AddTokenAsync(token, userId.ToString());
    }
}
