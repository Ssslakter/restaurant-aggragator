using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestaurantAggregator.Infra.Swagger;

public class AddAuthHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attributes = context.MethodInfo.GetCustomAttributes(true)
#nullable disable
            .Union(context.MethodInfo.DeclaringType.GetCustomAttributes(true))
#nullable enable
            .OfType<AuthorizeAttribute>();

        if (attributes.Any())
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            operation.Security ??= new List<OpenApiSecurityRequirement>();

            var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "jwt_auth" } };
            operation.Security.Add(new OpenApiSecurityRequirement { { scheme, new List<string>() } });
        }
    }
}