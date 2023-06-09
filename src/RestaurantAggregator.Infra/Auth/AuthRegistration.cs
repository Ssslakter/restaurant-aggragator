using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestaurantAggregator.Infra.Config;

namespace RestaurantAggregator.Infra.Auth;

public static class AuthRegistration
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
            var jwtConfig = services.BuildServiceProvider().GetRequiredService<IOptions<JwtConfiguration>>().Value;
            options.TokenValidationParameters = jwtConfig.GenerateTokenValidationParameters();
#nullable enable
        });
        return services;
    }

    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CookieConfiguration>(configuration.GetSection("CookieConfiguration"));
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            var cookieConfig = services.BuildServiceProvider()
            .GetRequiredService<IOptions<CookieConfiguration>>().Value;
            options.ExpireTimeSpan = cookieConfig.Lifetime;
            options.ClaimsIssuer = cookieConfig.Issuer;
            options.AccessDeniedPath = cookieConfig.AccessDeniedPath;
        });
        return services;
    }
}
