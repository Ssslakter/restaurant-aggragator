using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RestaurantAggregator.Infra.Config;

public static class ServicesExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
#nullable disable
            var jwtConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<JwtConfiguration>>().Value;
            options.TokenValidationParameters = jwtConfiguration.GenerateTokenValidationParameters();
#nullable enable
        });
        return services;
    }
}
