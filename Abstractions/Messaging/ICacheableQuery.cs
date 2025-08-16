namespace Alwalid.Cms.Api.Abstractions.Messaging
{
    public interface ICacheableQuery<TResponse> : IQuery<TResponse>
    {
        string CacheKey { get; }
        TimeSpan? CacheDuration { get; }
    }

    public interface IInvalidateCacheCommand
    {
        IEnumerable<string> CacheKeys { get; }
    }
}
