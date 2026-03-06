using System.Text.Json.Serialization;

namespace HackerNewsApi.Models;

/// <summary>
/// Structured JSON response: error message compulsory for errors; for success message is the default status text (e.g. "OK" for 200) and optional generic data.
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

  public static ApiResponse<T> Success(T? data = default, string message = "OK") => new()
  {
    Error = false,
    Message = message,
    Data = data
  };

  public static ApiResponse<T> Failure(string errorMessage) => new()
  {
    Error = true,
    Message = errorMessage,
    Data = default
  };
}
