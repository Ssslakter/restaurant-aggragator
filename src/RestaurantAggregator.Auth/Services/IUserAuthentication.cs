using RestaurantAggregator.Auth.Data.DTO;
using RestaurantAggregator.Auth.Data.Entities;

namespace RestaurantAggregator.Auth.Services;

public interface IUserAuthentication
{
    Task<User> Login(string email, string password);
    Task<User> Register(RegistrationModel registrationModel);
    Task<User> ValidateRefreshToken(string token, Guid userId);
    Task ChangePassword(Guid userId, string oldPassword, string newPassword);
    Task Logout(Guid userId);
    Task<TokenModel> GenerateTokenPairAsync(User user);
}