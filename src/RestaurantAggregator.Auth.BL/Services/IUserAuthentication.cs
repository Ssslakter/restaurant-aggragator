using System.Security.Claims;
using RestaurantAggregator.Auth.DAL.Data.Entities;
using RestaurantAggregator.Core.Data.Auth;

namespace RestaurantAggregator.Auth.BL.Services;

public interface IUserAuthentication
{
    Task<User> Login(string email, string password);
    Task<User> Register(RegistrationModel registrationModel);
    Task<User> ValidateRefreshToken(string token, Guid userId);
    Task ChangePassword(Guid userId, string oldPassword, string newPassword);
    Task Logout(Guid userId);
    Task<TokenModel> GenerateTokenPairAsync(User user);
    Task<List<Claim>> GetClaims(User user);
}