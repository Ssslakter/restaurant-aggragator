using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace RestaurantAggregator.Infra.Swagger;

public static class ConfigureSwaggerExtensions
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("jwt_auth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT authorization header using the Bearer scheme",
            });

            options.OperationFilter<AddAuthHeaderOperationFilter>();
        });
        return services;
    }
}
