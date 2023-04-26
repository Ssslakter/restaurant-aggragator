using RestaurantAggregator.Auth.Client;
using RestaurantAggregator.MVC.Services.Client;

namespace RestaurantAggregator.MVC.Services;
#nullable disable
public static class Registration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserManager, UserManager>();
        services.AddHttpClient<IRestaurantApiClient, RestaurantApiClient>(client =>
                client.BaseAddress = new Uri(configuration["RestaurantApiUrl"]));
        return services;
    }
}
