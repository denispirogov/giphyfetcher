# GiphyFetcher

A simple ASP.NET Core application for searching and fetching trending GIFs from the Giphy API.

## Features

- **Get trending GIFs**
- **Search GIFs by term**
- **In-memory caching** to reduce redundant Giphy API calls
- **Safe concurrent requests** with per-key locking
- **Extensible provider-based architecture**
- **Simple web UI** for browsing GIFs

## Architecture

```
Controller
    ↓
GifService
    ├── ICacheService
    └── IGifProvider
            ↓
      GiphyProvider
            ↓
        Giphy API
```

The `IGifProvider` abstraction allows the Giphy integration to be replaced or extended with another GIF provider without changing the application layer.

## API Endpoints

- `GET /api/gifs/trending`
- `GET /api/gifs/search?term=cat`

## Configuration

Configure the Giphy API key using .NET configuration:

```json
{
  "Giphy": {
    "ApiKey": "YOUR_API_KEY"
  }
}
```

## Run

```bash
cd GiphyFetcher.Api
dotnet restore
dotnet run
```

Then open the application URL in a browser.

## Technologies

- **C#**
- **ASP.NET Core**
- **HttpClientFactory**
- **IMemoryCache**
- **HTML / CSS / JavaScript**
- **Giphy API**