using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.DAL.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RestaurantAggregator.Auth.DAL.Data;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Auth.Extensions;

internal static class DbRegistration
{
    internal static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Auth")));

        services.AddIdentity<User, Role>(o => o.Password.RequireNonAlphanumeric = false)
            .AddEntityFrameworkStores<AuthDbContext>();
        services.AddScoped<RoleManager<Role>>();
        services.AddScoped<UserManager<User>>();

        services.MigrateDatabase();
        services.AddRoles();
        return services;
    }

    private static IServiceCollection MigrateDatabase(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        context.Database.Migrate();

        return services;
    }

    private static IServiceCollection AddRoles(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        foreach (var role in Enum.GetValues(typeof(RoleType)).Cast<RoleType>())
        {
            if (!roleManager.RoleExistsAsync(role.ToString()).Result)
            {
                roleManager.CreateAsync(new Role { Name = role.ToString() }).Wait();
            }
        }
        return services;
    }
}
