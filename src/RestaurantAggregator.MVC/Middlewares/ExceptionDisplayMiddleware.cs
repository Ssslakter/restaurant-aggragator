using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.MVC.Middlewares;

public static class ExceptionDisplayMiddleware
{
    public static void UseErrorDisplayPage(this IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("ExceptionDisplayMiddleware");
        app.Use(async (context, next) =>
        {
            var errorMessage = "";
            try
            {
                await next();
                return;
            }
            catch (AuthException ex)
            {
                logger.LogInformation(ex, "AuthError");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorMessage = ex.Message;
            }
            catch (NotFoundInDbException ex)
            {
                logger.LogInformation(ex, "NotFoundInDb");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorMessage = ex.Message;
            }
            catch (DbViolationException ex)
            {
                logger.LogInformation(ex, "DbViolation");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorMessage = ex.Message;
            }
            catch (BusinessException ex)
            {
                logger.LogInformation(ex, "BusinessError");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorMessage = ex.Message;
            }
            catch (ForbidException ex)
            {
                logger.LogInformation(ex, "Forbidden");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                errorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ServerError");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorMessage = "Internal server error";
            }

            context.Response.Redirect($"/error?message={errorMessage}");
        });
    }
}
