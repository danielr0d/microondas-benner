using Microondas.Domain.Exceptions;
using System.Text.Json;

namespace Microondas.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly string _logFilePath;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _logFilePath = Path.Combine(AppContext.BaseDirectory, "Logs", "errors.txt");
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new { message = exception.Message };

        if (exception is InvalidHeatingTimeException || 
            exception is InvalidPowerException ||
            exception is InvalidHeatingCharacterException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            LogErrorToFile(exception);
        }

        return context.Response.WriteAsJsonAsync(response);
    }

    private void LogErrorToFile(Exception exception)
    {
        try
        {
            var logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory!);

            var logMessage = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {exception.GetType().Name}: {exception.Message}\n{exception.StackTrace}\n\n";
            File.AppendAllText(_logFilePath, logMessage);
        }
        catch
        {
        }
    }
}

