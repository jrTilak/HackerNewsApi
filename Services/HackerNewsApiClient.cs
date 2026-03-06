using System.Net.Http.Json;
using HackerNewsApi.Models;

namespace HackerNewsApi.Services;

/// <summary>
/// HTTP client for Hacker News Firebase API (https://github.com/HackerNews/API).
/// </summary>
public class HackerNewsApiClient : IHackerNewsApiClient
{
  private readonly HttpClient _httpClient;
  private const string BestStoriesPath = "v0/beststories.json";
  private const string ItemPathTemplate = "v0/item/{0}.json";

  public HackerNewsApiClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<long[]?> GetBestStoryIdsAsync(CancellationToken cancellationToken = default)
  {
    var response = await _httpClient.GetAsync(BestStoriesPath, cancellationToken);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<long[]>(cancellationToken);
  }

  public async Task<HackerNewsItem?> GetItemAsync(long id, CancellationToken cancellationToken = default)
  {
    var path = string.Format(ItemPathTemplate, id);
    var response = await _httpClient.GetAsync(path, cancellationToken);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<HackerNewsItem>(cancellationToken);
  }
}
