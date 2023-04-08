using System.Net;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Auth.Middlewares;

public record ErrorResponse(string Message);

public static class ExceptionHandlingMiddleware
{
    public static void UseExceptionLogging(this IApplicationBuilder app, ILogger logger)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (AuthException ex)
            {
                logger.LogInformation(ex, "AuthError");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new ErrorResponse(ex.Message));
            }
            catch (NotFoundInDbException ex)
            {
                logger.LogWarning(ex, "NotFoundInDb");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsJsonAsync(new ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ServerError");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        });
    }
}
