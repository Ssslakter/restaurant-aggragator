using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAggregator.Auth.Client.Services;

namespace RestaurantAggregator.Auth.Client;
#nullable disable
public static class Registration
{
    public static IServiceCollection AddAuthApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
        client.BaseAddress = new Uri(configuration["AuthApiUrl"]));
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<TokenInfo>();
        return services;
    }

    /// <summary>
    /// Add services for authorization. Requires AddAuthApiClient, service account for AuthApi and AuthApiSecret section in appsettings.json
    /// </summary>
    public static IServiceCollection AddServiceAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthApiSecret>(configuration.GetSection("AuthApiSecret"));
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
