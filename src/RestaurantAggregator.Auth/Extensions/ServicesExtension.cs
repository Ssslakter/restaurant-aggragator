using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Data;
using RestaurantAggregator.Auth.Data.Entities;
using RestaurantAggregator.Auth.Services;
using StackExchange.Redis;
using Role = RestaurantAggregator.Auth.Data.Entities.Role;

namespace RestaurantAggregator.Auth.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddTransient<IJwtAuthentication, JwtAuthentication>();
        services.AddDatabases(configuration);
        var jwtAuthentication = services.BuildServiceProvider().GetRequiredService<IJwtAuthentication>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => options.TokenValidationParameters = jwtAuthentication.GenerateTokenValidationParameters());

        services.AddAuthorization();
        services.AddScoped<IUserAuthentication, UserAuthentication>();
        return services;
    }

    public static IServiceCollection MigrateDatabase(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            context.Database.Migrate();
            logger.LogInformation("Migrated database associated with context {DbContextName}", nameof(AuthDbContext));
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "An error occurred while migrating the database.");
        }
        return services;
    }

    private static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Auth")));

        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
#nullable disable
            var configurationOptions = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
#nullable enable
            return ConnectionMultiplexer.Connect(configurationOptions);
        });

        services.AddIdentity<User, Role>(o => o.Password.RequireNonAlphanumeric = false)
            .AddEntityFrameworkStores<AuthDbContext>();
        services.AddScoped<RoleManager<Role>>();
        services.AddScoped<UserManager<User>>();
        return services;
    }
}
