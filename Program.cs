using HackerNewsApi.Endpoints;
using HackerNewsApi.Models;
using HackerNewsApi.Options;
using HackerNewsApi.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();

builder.Services.Configure<HackerNewsOptions>(builder.Configuration.GetSection(HackerNewsOptions.SectionName));
builder.Services.Configure<BestStoriesOptions>(builder.Configuration.GetSection(BestStoriesOptions.SectionName));

builder.Services.AddHttpClient<IHackerNewsApiClient, HackerNewsApiClient>((sp, client) =>
{
  var options = sp.GetRequiredService<IOptions<HackerNewsOptions>>().Value;
  client.BaseAddress = new Uri(options.BaseAddress.TrimEnd('/') + "/");
});
builder.Services.AddScoped<IBestStoriesService, BestStoriesService>();

var app = builder.Build();

app.MapBestStories();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
  });
}

app.UseHttpsRedirection();

// Ensure all responses are application/json
app.Use(async (HttpContext context, RequestDelegate next) =>
{
  context.Response.OnStarting(() =>
  {
    if (!context.Response.HasStarted && context.Response.ContentType is null)
      context.Response.ContentType = "application/json";
    return Task.CompletedTask;
  });
  await next(context);
});

// Return 404 JSON for any unmatched route
app.MapFallback(async context =>
{
  context.Response.StatusCode = StatusCodes.Status404NotFound;
  await context.Response.WriteAsJsonAsync(ApiResponse<object?>.Failure("Not Found"));
});

app.Run();

