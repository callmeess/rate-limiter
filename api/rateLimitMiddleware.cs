namespace rateLimit;

public class rateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRateLimitService _rateLimitService;

    public rateLimitMiddleware(RequestDelegate next, IRateLimitService rateLimitService)
    {
        _next = next;
        _rateLimitService = rateLimitService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_rateLimitService.IsRequestAllowedAsync().Result)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Request denied due to rate limiting.");
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Too Many Requests");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Request allowed by rate limiter.");
        await _next(context);
    }
}