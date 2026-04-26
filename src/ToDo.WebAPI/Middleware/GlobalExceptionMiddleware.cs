using System.Text.Json;
using ToDo.Application.DTOs;

namespace ToDo.WebAPI.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Request processing error");

        var response = new ErrorResponse
        {
            Message = "An error occurred",
            Detail = exception.Message,
            Timestamp = DateTime.UtcNow
        };

        switch (exception)
        {
            case ArgumentException:
                context.Response.StatusCode = 400;
                response.Message = "Invalid request";
                break;

            case KeyNotFoundException:
                context.Response.StatusCode = 404;
                response.Message = "Resource not found";
                break;

            default:
                context.Response.StatusCode = 500;
                response.Message = "Internal server error";
                if (!_env.IsDevelopment())
                    response.Detail = null;
                break;
        }

        context.Response.ContentType = "application/json";

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}