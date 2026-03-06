using System.Text.Json.Serialization;

namespace HackerNewsApi.Models;

/// <summary>
/// Raw item from Hacker News Firebase API (v0/item/{id}.json).
/// </summary>
public record HackerNewsItem(
  [property: JsonPropertyName("id")] long Id,
  [property: JsonPropertyName("title")] string? Title,
  [property: JsonPropertyName("url")] string? Url,
  [property: JsonPropertyName("by")] string? By,
  [property: JsonPropertyName("time")] long? Time,
  [property: JsonPropertyName("score")] int? Score,
  [property: JsonPropertyName("descendants")] int? Descendants,
  [property: JsonPropertyName("type")] string? Type
);
