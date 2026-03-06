using System.Text.Json.Serialization;

namespace HackerNewsApi.Models;

/// <summary>
/// Best story as returned by this API (Santander spec format).
/// </summary>
public record BestStoryDto(
  [property: JsonPropertyName("title")] string Title,
  [property: JsonPropertyName("uri")] string Uri,
  [property: JsonPropertyName("postedBy")] string PostedBy,
  [property: JsonPropertyName("time")] string Time,
  [property: JsonPropertyName("score")] int Score,
  [property: JsonPropertyName("commentCount")] int CommentCount
);
