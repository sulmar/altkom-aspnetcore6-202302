namespace Altkom.Shopper.Api.Middlewares;

public static class LoggerMiddlewareExtensions
{
    public static WebApplication UseLogger(this WebApplication app)
    {
        app.UseMiddleware<LoggerMiddleware>();

        return app;
    }
}

public class LoggerMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<LoggerMiddleware> logger;

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
	{
        this.next = next;
        this.logger = logger;
    }

	public async Task InvokeAsync(HttpContext context)
	{
        // na wejściu (żądanie)
        logger.LogInformation("{Method} {Path}", context.Request.Method, context.Request.Path);

        // następnik 
        await next(context);

        // na wyjściu (odpowiedź)
        logger.LogInformation("{StatusCode}", context.Response.StatusCode);
    }
}
