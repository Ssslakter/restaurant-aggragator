using Microsoft.Extensions.Logging;
using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Auth.Client.Services;

public class UserService
{
    private readonly IAuthApiClient _authApiClient;
    private readonly IAuthService _authService;
    private readonly ILogger<UserService> _logger;

    public UserService(IAuthApiClient authApiClient, IAuthService authService, ILogger<UserService> logger)
    {
        _authApiClient = authApiClient;
        _authService = authService;
        _logger = logger;
    }

    public async Task<ProfileCreation> GetProfile()
    {
        var token = await _authService.GetTokenAsync();
        return await _authApiClient.ProfileAsync();
    }
}
