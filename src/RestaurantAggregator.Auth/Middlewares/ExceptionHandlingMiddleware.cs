using System.Net;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Auth.Middlewares;

public record ErrorResponse(string Message);

public static class ExceptionHandlingMiddleware
{
    public static void UseExceptionLogging(this IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("ExceptionHandlingMiddleware");
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
                logger.LogInformation(ex, "NotFoundInDb");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsJsonAsync(new ErrorResponse(ex.Message));
            }
            catch (DbViolationException ex)
            {
                logger.LogInformation(ex, "DbViolation");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ServerError");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new ErrorResponse("Internal server error"));
            }
        });
    }
}
