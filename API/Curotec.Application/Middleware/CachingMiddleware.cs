using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

public class CachingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;

    public CachingMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Get)
        {
            var cacheKey = context.Request.Path.ToString();
            if (_cache.TryGetValue(cacheKey, out var cachedResponse))
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(cachedResponse.ToString());
                return;
            }

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _cache.Set(cacheKey, responseBodyText, TimeSpan.FromMinutes(5)); //TODO: move this fixed value to a config file

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        else
        {
            await _next(context);
        }
    }
}