using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using HouseBroker.Application.Common;
using HouseBroker.Application.CustomException;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace HouseBroker.Application.Middlewares;

public class ExceptionHandlerMiddlewareMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
           await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, errorMessages) = exception switch
        {
            BadRequestException badRequestException =>
                (HttpStatusCode.BadRequest, [badRequestException.Message]),

            UnauthorizedAccessException unauthorizedException =>
                (HttpStatusCode.Unauthorized,[unauthorizedException.Message]),
            
            _ => (HttpStatusCode.InternalServerError,
                new List<string> { "Internal Server Error from the custom middleware." })
        };
        var apiResponse = new APIResponse(
            data: null,
            errors: errorMessages,
            statusCode: statusCode
        );
        await context.Response.WriteAsync(JsonSerializer.Serialize(apiResponse));
    }
    
}

public static class ExceptionHandlerMiddlewareMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddlewareMiddleware>();
    }
}