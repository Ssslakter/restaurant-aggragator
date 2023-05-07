using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.Auth.DAL.Data.Entities;
using RestaurantAggregator.Auth.BL.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace RestaurantAggregator.Auth.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddUserServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IJwtAuthentication, JwtAuthentication>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IUserAuthentication, UserAuthentication>();
        services.AddScoped<IRolesService, RolesService>();
        services.AddScoped<IBanService, BanService>();

        services.RegisterDbContext(configuration);

        return services;
    }
}
