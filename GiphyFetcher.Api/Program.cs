using GiphyFetcher.Domain.Interfaces;
using GiphyFetcher.Domain.Services;
using GiphyFetcher.Infrastructure.Caching;
using GiphyFetcher.Infrastructure.Giphy;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.Configure<GiphyOptions>(
    builder.Configuration.GetSection("Giphy"));

builder.Services.AddHttpClient<IGifProvider, GiphyProvider>(
    client =>
    {
        client.BaseAddress =
            new Uri("https://api.giphy.com/");
        client.Timeout = TimeSpan.FromSeconds(10);
    });

builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

builder.Services.AddScoped<IGifService, GifService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
