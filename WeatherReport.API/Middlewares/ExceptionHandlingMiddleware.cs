using System.Net;
using WeatherReport.Business.DTOs;

namespace WeatherReport.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext,ex.Message,HttpStatusCode.InternalServerError,"Something gone wrong");
        }
    }
    private async Task HandleExceptionAsync(HttpContext context,string exMessage,
                                            HttpStatusCode httpStatusCode,string message)
    {
        logger.LogError(exMessage);

        HttpResponse response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)httpStatusCode;

        var errorDTO = new ErrorDTO
        {
            Message = message,
            StatusCode = (int)httpStatusCode
        };

        string result = errorDTO.ToString();

        await response.WriteAsJsonAsync(result);
    }
}