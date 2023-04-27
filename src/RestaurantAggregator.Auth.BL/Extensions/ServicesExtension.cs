using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.DAL.Data.Entities;
using RestaurantAggregator.Auth.BL.Services;
using Role = RestaurantAggregator.Auth.DAL.Data.Entities.Role;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RestaurantAggregator.Auth.DAL.Data;

namespace RestaurantAggregator.Auth.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IJwtAuthentication, JwtAuthentication>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IUserAuthentication, UserAuthentication>();
        services.AddScoped<IRolesService, RolesService>();

        return services;
    }

    public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Auth")));

        services.AddIdentity<User, Role>(o => o.Password.RequireNonAlphanumeric = false)
            .AddEntityFrameworkStores<AuthDbContext>();
        services.AddScoped<RoleManager<Role>>();
        services.AddScoped<UserManager<User>>();

        services.MigrateDatabase();
        return services;
    }

    private static IServiceCollection MigrateDatabase(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        context.Database.Migrate();

        return services;
    }
}
