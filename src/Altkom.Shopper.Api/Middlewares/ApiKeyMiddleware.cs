namespace Altkom.Shopper.Api.Middlewares;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        this.next = next;        
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-ApiKey", out var apikey) && apikey == "123")
        {
            await next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
    }

}