using HackerNewsApi.Models;
using HackerNewsApi.Options;
using HackerNewsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsApi.Endpoints;

/// <summary>
/// Best stories from Hacker News API (Santander coding test).
/// </summary>
public static class BestStoriesEndpoints
{
  public static IEndpointRouteBuilder MapBestStories(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/beststories")
      .WithTags("Best Stories");

    group.MapGet("/", GetBestStories)
      .WithName("GetBestStories")
      .WithSummary("Returns the best n stories from Hacker News by score.")
      .WithDescription("Fetches story IDs from HN beststories, then item details. Results are cached to avoid overloading the Hacker News API. Sorted by score descending.")
      .Produces<ApiResponse<BestStoryDto[]>>(StatusCodes.Status200OK, "application/json")
      .Produces<ApiResponse<object?>>(StatusCodes.Status400BadRequest, "application/json");

    return app;
  }

  private static async Task<IResult> GetBestStories(
    [FromServices] IBestStoriesService service,
    [FromServices] Microsoft.Extensions.Options.IOptions<BestStoriesOptions> options,
    [FromQuery] int n = 10,
    CancellationToken cancellationToken = default)
  {
    if (n < 1 || n > options.Value.MaxStoriesPerRequest)
    {
      return Results.BadRequest(ApiResponse<object?>.Failure($"n must be between 1 and {options.Value.MaxStoriesPerRequest}."));
    }

    var stories = await service.GetBestStoriesAsync(n, cancellationToken);
    return Results.Ok(ApiResponse<BestStoryDto[]>.Success(stories.ToArray()));
  }
}
