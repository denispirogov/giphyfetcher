using System.Collections.Concurrent;
using GiphyFetcher.Domain.Interfaces;
using GiphyFetcher.Domain.Models;

namespace GiphyFetcher.Domain.Services;

public sealed class GifService : IGifService
{
    private readonly IGifProvider _provider;
    private readonly ICacheService _cache;

    private readonly ConcurrentDictionary<string, SemaphoreSlim>
        _locks = new();

    public GifService(
        IGifProvider provider,
        ICacheService cache)
    {
        _provider = provider;
        _cache = cache;
    }

    public Task<IReadOnlyList<GifResult>> GetTrendingAsync(
        CancellationToken cancellationToken)
    {
        return GetOrFetchAsync(
            "trending",
            () => _provider.GetTrendingAsync(cancellationToken),
            cancellationToken);
    }

    public Task<IReadOnlyList<GifResult>> SearchAsync(
        string term,
        CancellationToken cancellationToken)
    {
        var normalized = Normalize(term);

        if (string.IsNullOrWhiteSpace(normalized))
            throw new ArgumentException(
                "Search term cannot be empty.",
                nameof(term));

        return GetOrFetchAsync(
            $"search:{normalized}",
            () => _provider.SearchAsync(
                normalized,
                cancellationToken),
            cancellationToken);
    }

    private async Task<IReadOnlyList<GifResult>> GetOrFetchAsync(
        string key,
        Func<Task<IReadOnlyList<GifResult>>> factory,
        CancellationToken cancellationToken)
    {
        var cached =
            await _cache.GetAsync<IReadOnlyList<GifResult>>(
                key,
                cancellationToken);

        if (cached is not null)
            return cached;

        var semaphore = _locks.GetOrAdd(
            key,
            _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync(cancellationToken);

        try
        {
            cached =
                await _cache.GetAsync<IReadOnlyList<GifResult>>(
                    key,
                    cancellationToken);

            if (cached is not null)
                return cached;

            var result = await factory();

            await _cache.SetAsync(
                key,
                result,
                TimeSpan.FromMinutes(10),
                cancellationToken);

            return result;
        }
        finally
        {
            semaphore.Release();
        }
    }

    private static string Normalize(string value)
    {
        return value.Trim().ToLowerInvariant();
    }
}