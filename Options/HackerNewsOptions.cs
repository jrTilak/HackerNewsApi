namespace HackerNewsApi.Options;

/// <summary>
/// Configuration for Hacker News API base address.
/// </summary>
public class HackerNewsOptions
{
  public const string SectionName = "HackerNews";

  /// <summary>Base URL for the Hacker News Firebase API (no trailing slash). Configure in appsettings.json under HackerNews:BaseAddress.</summary>
  public string BaseAddress { get; set; } = "";
}
