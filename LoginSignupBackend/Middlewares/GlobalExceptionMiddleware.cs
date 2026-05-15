using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;
using System.Text.Json;

namespace LoginSignupBackend.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<GlobalExceptionMiddleware> _logger;
        //private readonly string _logFilePath;


        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            //_logger = logger;
            //_logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "exceptions.txt");
            //Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath)!);
            Log.Information("GlobalExceptionMiddleware initialized.");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //await LogToFileAsync(ex,context);
                //await HandleExceptionAsync(context, ex);

                Log.Error(ex,
                   "Unhandled Exception | Endpoint: {Method} {Path} | Type: {ExType} | Message: {Message}",
                   context.Request.Method,
                   context.Request.Path,
                   ex.GetType().Name,
                   ex.Message
               );

                await HandleExceptionAsync(context, ex);

            }
        }
        //private async Task LogToFileAsync(Exception ex,HttpContext context)
        //{
        //    var logEntry = $"""
        //        ============================================================
        //        Timestamp  : {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC
        //        Endpoint   : {context.Request.Method} {context.Request.Path}
        //        Exception  : {ex.GetType().Name}
        //        Message    : {ex.Message}
        //        StackTrace : {ex.StackTrace}
        //        ============================================================

        //        """;

        //    await File.AppendAllTextAsync(_logFilePath, logEntry);
        //}
        //private static async Task HandleExceptionAsync(HttpContext context,Exception ex)
        //{
        //    context.Response.ContentType="application/json";
        //    context.Response.StatusCode = ex.Message switch
        //    {
        //        var m when m.Contains("not found", StringComparison.OrdinalIgnoreCase)
        //            => (int)HttpStatusCode.NotFound,
        //        var m when m.Contains("Access denied", StringComparison.OrdinalIgnoreCase)
        //            => (int)HttpStatusCode.Forbidden,
        //        var m when m.Contains("Invalid", StringComparison.OrdinalIgnoreCase)
        //            => (int)HttpStatusCode.BadRequest,
        //        var m when m.Contains("already registered", StringComparison.OrdinalIgnoreCase)
        //            => (int)HttpStatusCode.Conflict,
        //        _ => (int)HttpStatusCode.InternalServerError
        //    };

        //    var response = new
        //    {
        //        error = ex.Message
        //    };

        //    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        //}
        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex.Message switch
            {
                var m when m.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    => (int)HttpStatusCode.NotFound,
                var m when m.Contains("Access denied", StringComparison.OrdinalIgnoreCase)
                    => (int)HttpStatusCode.Forbidden,
                var m when m.Contains("Invalid", StringComparison.OrdinalIgnoreCase)
                    => (int)HttpStatusCode.BadRequest,
                var m when m.Contains("already registered", StringComparison.OrdinalIgnoreCase)
                    => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new { error = ex.Message };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

    }
}
