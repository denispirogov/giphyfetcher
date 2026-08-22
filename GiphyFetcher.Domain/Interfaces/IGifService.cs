using GiphyFetcher.Domain.Models;

namespace GiphyFetcher.Domain.Interfaces;

public interface IGifService
{
    Task<IReadOnlyList<GifResult>> GetTrendingAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GifResult>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}