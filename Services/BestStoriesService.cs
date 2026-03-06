using HackerNewsApi.Models;
using HackerNewsApi.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HackerNewsApi.Services;

/// <summary>
/// Fetches best stories from HN API with caching to avoid overloading the upstream API.
/// </summary>
public class BestStoriesService : IBestStoriesService
{
  private const string BestStoryIdsCacheKey = "hn:beststories:ids";
  private const string ItemCacheKeyPrefix = "hn:item:";

  private readonly IHackerNewsApiClient _apiClient;
  private readonly IMemoryCache _cache;
  private readonly BestStoriesOptions _options;

  public BestStoriesService(
    IHackerNewsApiClient apiClient,
    IMemoryCache cache,
    IOptions<BestStoriesOptions> options)
  {
    _apiClient = apiClient;
    _cache = cache;
    _options = options.Value;
  }

  public async Task<IReadOnlyList<BestStoryDto>> GetBestStoriesAsync(int n, CancellationToken cancellationToken = default)
  {
    var idsCacheDuration = TimeSpan.FromMinutes(_options.BestStoryIdsCacheDurationMinutes);
    var ids = await _cache.GetOrCreateAsync(BestStoryIdsCacheKey, async entry =>
    {
      entry.AbsoluteExpirationRelativeToNow = idsCacheDuration;
      return await _apiClient.GetBestStoryIdsAsync(cancellationToken);
    });

    if (ids is null || ids.Length == 0)
      return Array.Empty<BestStoryDto>();

    var take = Math.Min(n, ids.Length);
    var tasks = ids.Take(take).Select(id => GetItemCachedAsync(id, cancellationToken));
    var items = await Task.WhenAll(tasks);

    var dtos = items
      .Where(i => i is not null && IsStoryWithScore(i))
      .Select(i => MapToDto(i!))
      .OrderByDescending(s => s.Score)
      .Take(n)
      .ToList();

    return dtos;
  }

  private async Task<HackerNewsItem?> GetItemCachedAsync(long id, CancellationToken cancellationToken)
  {
    var key = ItemCacheKeyPrefix + id;
    var duration = TimeSpan.FromMinutes(_options.ItemCacheDurationMinutes);
    return await _cache.GetOrCreateAsync(key, async entry =>
    {
      entry.AbsoluteExpirationRelativeToNow = duration;
      return await _apiClient.GetItemAsync(id, cancellationToken);
    });
  }

  private static bool IsStoryWithScore(HackerNewsItem item) =>
    item.Score.HasValue && !string.IsNullOrEmpty(item.Title);

  private static BestStoryDto MapToDto(HackerNewsItem item)
  {

    //time should be in UTC timezone
    var time = item.Time.HasValue
      ? DateTimeOffset.FromUnixTimeSeconds(item.Time.Value).UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss'+00:00'")
      : "";
    return new BestStoryDto(
      Title: item.Title ?? "",
      Uri: item.Url,
      PostedBy: item.By ?? "",
      Time: time,
      Score: item.Score ?? 0,
      CommentCount: item.Descendants ?? 0
    );
  }
}
