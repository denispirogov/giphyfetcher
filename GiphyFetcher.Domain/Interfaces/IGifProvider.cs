using GiphyFetcher.Domain.Models;

namespace GiphyFetcher.Domain.Interfaces;

public interface IGifProvider
{
    Task<IReadOnlyList<GifResult>> GetTrendingAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyList<GifResult>> SearchAsync(
        string term,
        CancellationToken cancellationToken);
}