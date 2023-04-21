using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace RestaurantAggregator.Infra.Swagger;

public static class ConfigureSwaggerExtensions
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services, Assembly assembly)
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

            var xmlFile = $"{assembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
        });
        return services;
    }
}
