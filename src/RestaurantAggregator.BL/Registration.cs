using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAggregator.BL.Services;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL.Extensions;

namespace RestaurantAggregator.BL;

public static class Registration
{
    public static void RegisterBLServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDishService, DishService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IRestaurantService, RestaurantService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.RegisterDbContext(configuration);
    }
}
