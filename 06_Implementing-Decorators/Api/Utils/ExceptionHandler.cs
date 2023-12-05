using System.Net;
using System.Text.Json;

namespace Api.Utils;

public sealed class ExceptionHandler(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (!context.Request.Path.StartsWithSegments("/api"))
                throw;

            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Log exception here
        string result = JsonSerializer.Serialize(Envelope.Error(exception.Message));
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(result);
    }
}
