using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;

namespace RestaurantAggregator.BL;

public static class Registration
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDishService, DishService>();
        services.RegisterDbContext(configuration);
    }
}
