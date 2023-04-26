using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.Auth.Data.Entities;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Auth.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Auth.Services;

public class UserAuthentication : IUserAuthentication
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtAuthentication _jwtAuthentication;
    //private readonly ILogger<UserAuthentication> _logger;

    public UserAuthentication(UserManager<User> userManager, IJwtAuthentication jwtAuthentication)
    {
        _userManager = userManager;
        _jwtAuthentication = jwtAuthentication;
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
            UserName = registrationModel.Email,
            Phone = registrationModel.Phone
        };
        var result = await _userManager.CreateAsync(user, registrationModel.Password);
        if (result.Errors.Any())
        {
            throw new AuthException(result.Errors.First().Description);
        }
        var roleResult = await _userManager.AddToRoleAsync(user, nameof(RoleType.Client));
        if (roleResult.Errors.Any())
        {
            throw new AuthException(roleResult.Errors.First().Description);
        }
        return user;
    }

    public async Task Logout(Guid userId)
    {
        await _jwtAuthentication.RevokeAllUserRefreshTokensAsync(userId);
    }

    public async Task<User> ValidateRefreshToken(string token, Guid userId)
    {
        if (!await _jwtAuthentication.ValidateRefreshTokenAsync(token))
        {
            throw new AuthException("Invalid refresh token");
        }
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new NotFoundInDbException("User not found");
        }
        return user;
    }

    public async Task ChangePassword(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new NotFoundInDbException("User not found");
        }
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!result.Succeeded)
        {
            throw new AuthException(result.Errors.First().Description);
        }
    }

    public async Task<TokenModel> GenerateTokenPairAsync(User user)
    {
        var accessToken = await GenerateAccessTokenAsync(user);
        var refreshToken = await _jwtAuthentication.GenerateRefreshTokenAsync(user.Id);
        return new TokenModel
        {
            AccessToken = accessToken.Token,
            AccessTokenExpires = accessToken.Expires,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpires = refreshToken.Expires,
            UserId = user.Id
        };
    }

    private async Task<AccessToken> GenerateAccessTokenAsync(User user)
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
        return _jwtAuthentication.GenerateAccessToken(claims);
    }
}
