using GiphyFetcher.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace GiphyFetcher.Infrastructure.Caching;

public class MemoryCacheService : ICacheService
{
    // TODO: MemoryCache is instance-based. For production/distributed deployments,
    // consider using Redis as a shared cache with Polly for failure retries.
    
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
        _cache.TryGetValue(key, out T? value);

        return Task.FromResult(value);
    }

    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan expiration,
        CancellationToken cancellationToken = default)
    {
        _cache.Set(key, value, expiration);

        return Task.CompletedTask;
    }
}