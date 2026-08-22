using System.Net.Http.Json;
using GiphyFetcher.Domain.Interfaces;
using GiphyFetcher.Domain.Models;
using GiphyFetcher.Infrastructure.Giphy.Models;
using Microsoft.Extensions.Options;

namespace GiphyFetcher.Infrastructure.Giphy;

public class GiphyProvider : IGifProvider
{
    private readonly HttpClient _httpClient;
    private readonly GiphyOptions _options;

    public GiphyProvider(
        HttpClient httpClient,
        IOptions<GiphyOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<IReadOnlyList<GifResult>> GetTrendingAsync(
        CancellationToken cancellationToken)
    {
        var url =
            $"v1/gifs/trending?api_key={_options.ApiKey}&limit=50";

        var response = await _httpClient
            .GetFromJsonAsync<GiphyResponse>(
                url,
                cancellationToken);

        return response?.Data
                   .Select(x => new GifResult(
                       x.Id,
                       x.Images.Original.Url))
                   .ToList()
               ?? [];
    }

    public async Task<IReadOnlyList<GifResult>> SearchAsync(
        string term,
        CancellationToken cancellationToken)
    {
        var url =
            $"v1/gifs/search" +
            $"?api_key={_options.ApiKey}" +
            $"&q={Uri.EscapeDataString(term)}" +
            $"&limit=50";

        var response = await _httpClient
            .GetFromJsonAsync<GiphyResponse>(
                url,
                cancellationToken);
        
        return response?.Data
                   .Select(x => new GifResult(
                       x.Id,
                       x.Images.Original.Url))
                   .ToList()
               ?? [];
    }
    
    private static GifResult Map(GiphyGif gif)
    {
        return new GifResult(
            gif.Id,
            gif.Images.Original.Url);
    }
}