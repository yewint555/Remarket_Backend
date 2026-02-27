using System.Net;
using System.Text.Json;
using Application.ApiWrappers;
using Microsoft.IdentityModel.Tokens;

namespace Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var statusCode = exception switch
        {
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            SecurityTokenException => (int)HttpStatusCode.Unauthorized,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            FluentValidation.ValidationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var response = ApiResponse<string>.Failure(exception.Message, statusCode);

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(response, options);

        return context.Response.WriteAsync(result);
    }
}