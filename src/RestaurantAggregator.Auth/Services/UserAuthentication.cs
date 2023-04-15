using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.Auth.Data.Entities;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Auth.Data.DTO;
using RestaurantAggregator.Auth.Data;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Data.Enums;

namespace RestaurantAggregator.Auth.Services;

public interface IUserAuthentication
{
    Task<User> Login(string email, string password);
    Task<User> Register(RegistrationModel registrationModel);
    Task<User> FindByRefreshToken(string token);
    Task<TokenModel> GenerateTokenPairAsync(User user);
    Task Logout(Guid userId);
}

public class UserAuthentication : IUserAuthentication
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtAuthentication _jwtAuthentication;
    private readonly AuthDbContext _context;
    //private readonly ILogger<UserAuthentication> _logger;

    public UserAuthentication(UserManager<User> userManager, AuthDbContext context,
     IJwtAuthentication jwtAuthentication)
    {
        _userManager = userManager;
        _jwtAuthentication = jwtAuthentication;
        _context = context;
        //_logger = logger;
    }

    public async Task<User> Login(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            throw new NotFoundInDbException("User not found");
#nullable disable
        if (!await _userManager.CheckPasswordAsync(user, password))
            throw new AuthException("Password is incorrect");
#nullable enable
        return user;
    }

    public async Task<User> Register(RegistrationModel registrationModel)
    {
        var user = new User
        {
            Email = registrationModel.Email,
            Name = registrationModel.Name,
            FullName = $"{registrationModel.Surname} {registrationModel.Name} {registrationModel.MiddleName}",
            Phone = registrationModel.Phone,
            UserName = registrationModel.Email
        };
        user.Client = new Client { User = user };
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, registrationModel.Password);
        var result = await _userManager.CreateAsync(user);
        if (result.Errors.Any())
        {
            throw new AuthException(result.Errors.First().Description);
        }
        await _userManager.AddToRoleAsync(user, nameof(RoleType.Client));
        return user;
    }

    public async Task Logout(Guid userId)
    {
        await _context.RefreshTokens.Where(t => t.UserId == userId).ExecuteDeleteAsync();
    }

    public async Task<User> FindByRefreshToken(string token)
    {
        var userId = await _jwtAuthentication.GetUserIdFromTokenAsync(token);
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundInDbException("User not found");
        return user;
    }

    public async Task<TokenModel> GenerateTokenPairAsync(User user)
    {
        return new TokenModel
        {
            AccessToken = await GenerateAccessTokenAsync(user),
            RefreshToken = await GenerateRefreshTokenAsync(user)
        };
    }

    private async Task<string> GenerateAccessTokenAsync(User user)
    {
#nullable disable
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        foreach (var role in await _userManager.GetRolesAsync(user))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
#nullable enable
        return _jwtAuthentication.GenerateToken(claims);
    }

    private async Task<string> GenerateRefreshTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        _context.RefreshTokens.Add(new RefreshToken
        {
            Token = _jwtAuthentication.GenerateToken(claims, isRefreshToken: true),
            UserId = user.Id
        });
        await _context.SaveChangesAsync();
        return _jwtAuthentication.GenerateToken(claims, isRefreshToken: true);
    }
}
