using Microsoft.AspNetCore.Mvc;
using rateLimit;


[ApiController]
[Route("[controller]")]
public class TemperatureController : ControllerBase
{
    private readonly IRateLimitService _rateLimitService;
    private readonly string[] _summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };



    public TemperatureController(IRateLimitService rateLimitService)
    {

        _rateLimitService = rateLimitService;
    }




    [HttpGet("weatherforecast")]
    public async Task<IActionResult> GetTemperature()
    {

        // string key = "temperature_request";
        // int limit = 5; // Allow 5 requests
        // TimeSpan period = TimeSpan.FromMinutes(1); // per minute

        // bool isAllowed = await _rateLimitService.IsRequestAllowedAsync();

        // if (!isAllowed)
        // {
        //     Console.ForegroundColor = ConsoleColor.Red;
        //     Console.WriteLine("Temperature request denied due to rate limiting.");
        //     return StatusCode(429, "Too many requests. Please try again later.");

        // }

        var temperature = new Random().Next(-20, 55);
        return Ok(new { TemperatureC = temperature });
    }
}