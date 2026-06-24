namespace rateLimit;

public sealed class LeakyBucketRateLimiter : IRateLimitService
{
    private readonly object _lock = new();
    private readonly double _capacity;
    private readonly double _leakRatePerSecond;
    private double _currentLevel;
    private DateTime _lastUpdate;


    public LeakyBucketRateLimiter(
        int capacity,
        double leakRatePerSecond)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        if (leakRatePerSecond <= 0)
            throw new ArgumentOutOfRangeException(nameof(leakRatePerSecond));

        _capacity = capacity;
        _leakRatePerSecond = leakRatePerSecond;
        _lastUpdate = DateTime.UtcNow;
    }

    public Task<bool> IsRequestAllowedAsync()
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;

            var elapsedSeconds = (now - _lastUpdate).TotalSeconds;

            _currentLevel = Math.Max(
                0,
                _currentLevel - elapsedSeconds * _leakRatePerSecond);

            _lastUpdate = now;

            if (_currentLevel < _capacity)
            {
                _currentLevel++;
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }

}