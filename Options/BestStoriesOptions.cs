namespace HackerNewsApi.Options;

/// <summary>
/// Configuration for best stories API and caching.
/// </summary>
public class BestStoriesOptions
{
  public const string SectionName = "BestStories";

  /// <summary>Maximum number of stories a caller can request in one call.</summary>
  public int MaxStoriesPerRequest { get; set; } = 100;

  /// <summary>Cache duration for the best story IDs list (minutes).</summary>
  public int BestStoryIdsCacheDurationMinutes { get; set; } = 5;

  /// <summary>Cache duration for individual item details (minutes).</summary>
  public int ItemCacheDurationMinutes { get; set; } = 5;
}
