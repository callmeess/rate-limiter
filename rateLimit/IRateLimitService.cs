
namespace rateLimit;
public interface IRateLimitService
{
    Task<bool> IsRequestAllowedAsync(string key, int limit, TimeSpan period);
    Task<bool> IsRequestAllowedAsync();
}