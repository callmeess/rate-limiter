namespace rateLimit;

public class TokenBucketLimmiter : IRateLimitService
{
    private readonly int _tokenBucketSize;
    private readonly double _refillRatePerSecond;
    private readonly object _lock = new();
    private double _currentLevel;
    private DateTime _lastUpdate = DateTime.UtcNow;




    public TokenBucketLimmiter(
        int tokenBuckets,
        double refillRatePerSecond)
    {
        if (tokenBuckets <= 0)
            throw new ArgumentOutOfRangeException(nameof(tokenBuckets));

        if (refillRatePerSecond <= 0)
            throw new ArgumentOutOfRangeException(nameof(refillRatePerSecond));

        _tokenBucketSize = tokenBuckets;
        _refillRatePerSecond = refillRatePerSecond;
    }


    public void RefillTokens()
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            var elapsedSeconds = (now - _lastUpdate).TotalSeconds;

            _currentLevel = Math.Min(
                _tokenBucketSize,
                _currentLevel + elapsedSeconds * _refillRatePerSecond);

            _lastUpdate = now;
        }

    }

    public void ConsumeToken()
    {
        lock (_lock)
        {
            if (_currentLevel > 0)
            {
                _currentLevel--;
            }
        }
    }


    public Task<bool> IsRequestAllowedAsync()
    {
        lock (_lock)
        {
            if (_currentLevel <= 0)
            {

                RefillTokens(); // no need to wait
                return Task.FromResult(false);
            }

        }

        ConsumeToken(); // no need to wait 
        RefillTokens(); // no need to wait
        return Task.FromResult(true);
    }
}