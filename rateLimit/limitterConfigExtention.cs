using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using rateLimit;

public static class LimiterConfigExtensions
{
    public static void AddLeakyBucketLimiter(this IServiceCollection services, IConfiguration configuration)
    {
        // config
        var leakyBucketConfig = configuration.GetSection("leakyBucket");
        int capacity = int.TryParse(leakyBucketConfig.GetSection("Capacity").Value, out var parsedCapacity) ? parsedCapacity : 0;
        int leakRate = int.TryParse(leakyBucketConfig.GetSection("LeakRate").Value, out var parsedLeakRate) ? parsedLeakRate : 0;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Leaky Bucket Config - Capacity: {capacity}, LeakRate: {leakRate}");

        // register
        services.AddSingleton<IRateLimitService>( new LeakyBucketRateLimiter(capacity, leakRate));
    }
}