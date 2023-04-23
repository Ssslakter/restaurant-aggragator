using Microsoft.Extensions.Logging;
using RestaurantAggregator.Core.Exceptions;
using RoleTypeCore = RestaurantAggregator.Core.Data.Enums.RoleType;

namespace RestaurantAggregator.Auth.Client.Services;

internal enum ResponseStatus
{
    Success = 200,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    InternalServerError = 500
}

public class UserService : IUserService
{
    private readonly IAuthApiClient _authApiClient;
    private readonly IAuthService _authService;
    private readonly TokenInfo _tokenInfo;
    private readonly ILogger<UserService> _logger;

    public UserService(IAuthApiClient authApiClient, IAuthService authService,
     ILogger<UserService> logger, TokenInfo tokenInfo)
    {
        _authApiClient = authApiClient;
        _authService = authService;
        _logger = logger;
        _tokenInfo = tokenInfo;
    }

    public async Task<ProfileDTO> GetProfileAsync()
    {
        await _authService.Authenticate();
        _authApiClient.SetToken(_tokenInfo.AccessToken);
        return await _authApiClient.ProfileAsync();
    }

    public async Task<ProfileDTO> GetProfileAsync(Guid userId)
    {
        await _authService.Authenticate();
        _authApiClient.SetToken(_tokenInfo.AccessToken);
        try
        {
            return await _authApiClient.Users2Async(userId);
        }
        catch (ApiException e)
        {
            switch ((ResponseStatus)e.StatusCode)
            {
                case ResponseStatus.NotFound:
                    _logger.LogWarning("Not found");
                    throw new NotFoundInDbException($"User with id {userId} not found");
                case ResponseStatus.Forbidden:
                    _logger.LogError(e, "Failed to get access to resource");
                    throw new ForbidException("Forbidden");
                default:
                    _logger.LogError(e, "Error");
                    throw;
            }
        }
    }

    public async Task<IEnumerable<RoleTypeCore>> GetUserRolesAsync(Guid userId)
    {
        await _authService.Authenticate();
        _authApiClient.SetToken(_tokenInfo.AccessToken);
        try
        {
            return (await _authApiClient.RolesAllAsync(userId))
            .Select(r => (RoleTypeCore)Enum.Parse(typeof(RoleTypeCore), r.ToString())).ToList();
        }
        catch (ApiException e)
        {
            switch ((ResponseStatus)e.StatusCode)
            {
                case ResponseStatus.NotFound:
                    _logger.LogWarning("Not found");
                    throw new NotFoundInDbException($"User with id {userId} not found");
                case ResponseStatus.Forbidden:
                    _logger.LogError(e, "Failed to get access to resource");
                    throw new ForbidException("Forbidden");
                default:
                    _logger.LogError(e, "Error");
                    throw;
            }
        }
    }
}
