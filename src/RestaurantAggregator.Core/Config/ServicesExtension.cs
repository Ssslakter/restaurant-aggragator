using Microsoft.Extensions.DependencyInjection;

namespace RestaurantAggregator.Core.Config;

public static class ServicesExtension
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IJwtConfiguration, JwtConfiguration>();
        return services;
    }
}
