using Alwalid.Cms.Api.Attributes;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly int _limit = 5;
        private readonly TimeSpan _window = TimeSpan.FromSeconds(60);

        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            // Check for metadata (only rate-limit if attribute is applied)
            if (endpoint == null || !endpoint.Metadata.Any(m => m is EnableRateLimitingAttribute))
            {
                await _next(context);
                return;
            }

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var path = context.Request.Path.Value?.ToLower() ?? "/";
            var key = $"rate:{ip}:{path}";

            var count = _cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _window;
                return 0;
            });

            count++;

            if (count > _limit)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Too many requests.");
                return;
            }

            _cache.Set(key, count, _window);
            await _next(context);
        }
    }
}
