namespace rateLimit;

// load configuration from appsettings.json
public class LeakyBucketLimiter : IRateLimitService
{
    private readonly int rate; // requests per second
    private int currentLevel;
    private DateTime lastChecked;


    public LeakyBucketLimiter(int rate , int currentLevel = 0)
    {
        this.rate = rate;
        this.currentLevel = currentLevel;
        this.lastChecked = DateTime.UtcNow;
    }

    public Task<bool> IsRequestAllowedAsync(string key, int limit, TimeSpan period)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsRequestAllowedAsync()
    {
        // lock to ensure thread safety
        lock (this)
        {
            if ((DateTime.UtcNow - lastChecked).TotalSeconds < 1.0 / rate)
            {
                return Task.FromResult(false);
            }
            currentLevel++;
            lastChecked = DateTime.UtcNow;
            return Task.FromResult(true);
        }

    }
}