using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantAggregator.DAL;

public static class ServicesExtension
{
    public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RestaurantDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Restaurant")));
        services.MigrateDatabase();
        return services;
    }

    private static IServiceCollection MigrateDatabase(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
        context.Database.Migrate();
        return services;
    }
}
