using System.Text.Json.Serialization;

namespace HackerNewsApi.Models;

/// <summary>
/// Structured JSON response: error message compulsory for errors; for success message is "normal" and optional generic data.
/// Content-Type is always application/json.
/// </summary>
public record ApiResponse<T>
{
  [JsonPropertyName("error")]
  public bool Error { get; init; }

  [JsonPropertyName("message")]
  public string Message { get; init; } = "";

  [JsonPropertyName("data")]
  public T? Data { get; init; }

  public static ApiResponse<T> Success(T? data = default) => new()
  {
    Error = false,
    Message = "normal",
    Data = data
  };

  public static ApiResponse<T> Failure(string errorMessage) => new()
  {
    Error = true,
    Message = errorMessage,
    Data = default
  };
}
