using Serilog;
using System.Text;
using System.Text.Json;

namespace Bns.Api.Middlewares;
public class SerilogRequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public SerilogRequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";

        // Логируем только для /api/auth/login и /api/auth/logout
        if (path == "/api/auth/login" || path == "/api/auth/logout")
        {
            var user = context.User.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "Anonymous";

            string requestBody = "";
            if(context.Request.HasFormContentType || context.Request.HasJsonContentType())
            {

                context.Request.EnableBuffering();
                context.Request.Body.Position = 0;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }

            Log.Information("HTTP {Method} {Path} by {User}. Body: {Body}",
                context.Request.Method,
                context.Request.Path,
                user,
                requestBody);

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {Method} {Path} by {User}. Body: {Body}",
                    context.Request.Method,
                    context.Request.Path,
                    user,
                    requestBody);
                throw;
            }

            if (context.Response.StatusCode >= 400)
            {
                Log.Warning("Request {Method} {Path} by {User} finished with status {StatusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    user,
                    context.Response.StatusCode);
            }
        }
        else
        {
            // Для остальных запросов просто вызываем следующий middleware
            await _next(context);
        }
    }
}