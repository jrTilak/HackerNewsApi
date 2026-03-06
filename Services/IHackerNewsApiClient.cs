namespace HackerNewsApi.Services;

/// <summary>
/// Client for the official Hacker News Firebase API.
/// </summary>
public interface IHackerNewsApiClient
{
  Task<long[]?> GetBestStoryIdsAsync(CancellationToken cancellationToken = default);
  Task<Models.HackerNewsItem?> GetItemAsync(long id, CancellationToken cancellationToken = default);
}
