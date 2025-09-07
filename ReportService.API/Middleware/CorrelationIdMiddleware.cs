using SharedKernel.Interfaces;
using System.Diagnostics;

namespace ReportService.API.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAppLogger<CorrelationIdMiddleware> logger)
    {
        var correlationId = context.Request.Headers.ContainsKey("X-Correlation-ID")
            ? context.Request.Headers["X-Correlation-ID"].ToString()
            : Guid.NewGuid().ToString();

        // Response’a da ekle
        //context.Response.Headers.Add("X-Correlation-ID", correlationId); //append ?
        context.Response.Headers["X-Correlation-ID"] = correlationId;

        context.Items["CorrelationId"] = correlationId;
        context.Items["X-Correlation-ID"] = correlationId; //diğer servislerde bu isimle kullanıldığı için ekledim.
        logger.SetCorrelationId(correlationId);

        //Activity/Diagnostics için
        Activity.Current?.SetTag("X-Correlation-ID", correlationId);

        await _next(context);
    }
}
