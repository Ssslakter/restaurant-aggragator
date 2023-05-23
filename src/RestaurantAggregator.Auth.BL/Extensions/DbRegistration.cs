using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.DAL.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RestaurantAggregator.Auth.DAL.Data;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Data.Auth;

namespace RestaurantAggregator.Auth.Extensions;

public static class DbRegistration
{
    internal static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Auth")));

        services.AddIdentity<User, Role>(o => o.Password.RequireNonAlphanumeric = false)
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddSignInManager<SignInManager<User>>();

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

    public static IServiceCollection AddAdmins(this IServiceCollection services, IConfiguration configuration)
    {
        //Add admin creds that in MVC configs and API configs, and add default admin user
        var creds = configuration.GetSection("DefaultAdminAccounts").GetChildren().Select(x => x.Get<LoginModel>()).ToList();
        using var scope = services.BuildServiceProvider().CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var role = roleManager.FindByNameAsync(nameof(RoleType.Admin)).Result;
#nullable disable
        foreach (var user in creds)
        {
            if (userManager.FindByEmailAsync(user.Email).Result == null)
            {
                var admin = new User
                {
                    Email = user.Email,
                    UserName = user.Email
                };
                var result = userManager.CreateAsync(admin, user.Password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, role.Name).Wait();
                }
            }
        }
#nullable enable
        return services;
    }
}
