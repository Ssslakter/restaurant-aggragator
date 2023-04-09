using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using RestaurantAggregator.Auth.Utils;

namespace RestaurantAggregator.Auth.Middlewares;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenStorage _tokenStorage;

    private readonly ILogger<TokenValidationMiddleware> _logger;

    public TokenValidationMiddleware(RequestDelegate next, ITokenStorage tokenStorage, ILogger<TokenValidationMiddleware> logger)
    {
        _next = next;
        _tokenStorage = tokenStorage;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>() == null)
        {
            await _next.Invoke(context);
            return;
        }
        var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(accessToken) && !await _tokenStorage.IsTokenActiveAsync(accessToken))
        {
            _logger.LogInformation("Token is not valid", accessToken);
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new { error = "Token is not valid" })));
            return;
        }

        await _next.Invoke(context);
    }
}

public static class TokenValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenValidationMiddleware>();
    }
}