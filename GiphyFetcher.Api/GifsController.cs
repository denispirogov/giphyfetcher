using GiphyFetcher.Domain.Interfaces;
using GiphyFetcher.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiphyFetcher.Api;

[ApiController]
[Route("api/gifs")]
public sealed class GifsController : ControllerBase
{
    private readonly IGifService _gifService;

    public GifsController(IGifService gifService)
    {
        _gifService = gifService;
    }

    [HttpGet("trending")]
    public async Task<ActionResult<IReadOnlyList<GifResult>>>
        GetTrending(CancellationToken cancellationToken)
    {
        var result = await _gifService
            .GetTrendingAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<GifResult>>>
        Search(
            [FromQuery] string term,
            CancellationToken cancellationToken)
    {
        var result = await _gifService
            .SearchAsync(term, cancellationToken);

        return Ok(result);
    }
}