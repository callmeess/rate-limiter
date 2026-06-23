using StackExchange.Redis;

public class RedisRateLimitService : IRateLimitService
{
    private readonly IDatabase _db;

    public RedisRateLimitService(IConnectionMultiplexer 
    connectionMultiplexer) => _db = connectionMultiplexer.GetDatabase();






    public async Task<bool> IsRequestAllowedAsync(
     string key,
     int limit,
     TimeSpan period)
    {
        long count = await _db.StringIncrementAsync(key);

        // first request, set the expiration time
        if (count == 1)
        {
            await _db.KeyExpireAsync(key, period);
        }


        // return true if the count is less than or equal to the limit
        return count <= limit;
    }
}