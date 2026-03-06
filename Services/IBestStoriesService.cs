using HackerNewsApi.Models;

namespace HackerNewsApi.Services;

/// <summary>
/// Orchestrates fetching best story IDs and item details, with caching and sorting.
/// </summary>
public interface IBestStoriesService
{
  Task<IReadOnlyList<BestStoryDto>> GetBestStoriesAsync(int n, CancellationToken cancellationToken = default);
}
