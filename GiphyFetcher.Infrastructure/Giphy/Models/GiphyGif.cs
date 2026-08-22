namespace GiphyFetcher.Infrastructure.Giphy.Models;

public sealed class GiphyGif
{
    public string Id { get; set; } = string.Empty;

    public GiphyImages Images { get; set; } = new();
}