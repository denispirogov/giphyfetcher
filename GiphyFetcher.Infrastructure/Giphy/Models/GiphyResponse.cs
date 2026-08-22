namespace GiphyFetcher.Infrastructure.Giphy.Models;

public sealed class GiphyResponse
{
    public List<GiphyGif> Data { get; set; } = [];
}