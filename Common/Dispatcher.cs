using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Abstractions;
using Alwalid.Cms.Api.Common.Handler;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Alwalid.Cms.Api.Services;

namespace Alwalid.Cms.Api.Common
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICacheService _cacheService;


        public Dispatcher(IServiceProvider serviceProvider, ICacheService cacheService)
        {
            _serviceProvider = serviceProvider;
            _cacheService = cacheService;

        }

        public async Task<Result<TResponse>> SendCommandAsync<TResponse>(
            ICommand<TResponse> command,
            CancellationToken cancellationToken = default
        )
        {
            var handlerType = typeof(ICommandHandler<,>)
                .MakeGenericType(command.GetType(), typeof(TResponse));

            var handler = _serviceProvider.GetService(handlerType);
            if (handler == null)
                return await Result<TResponse>.FaildAsync(false, $"No handler found for query {command.GetType().Name}");

            var method = handlerType.GetMethod("Handle");
            var result = await (Task<Result<TResponse>>)method.Invoke(handler, new object[] { command, cancellationToken });

            if (command is IInvalidateCacheCommand invalidator && result.IsSuccess)
            {
                foreach (var key in invalidator.CacheKeys)
                    await _cacheService.RemoveAsync(key);
            }

            return result;
        }

        public async Task<Result<TResponse>> SendQueryAsync<TResponse>(
            IQuery<TResponse> query,
            CancellationToken cancellationToken = default
        )
        {

            if (query is ICacheableQuery<TResponse> cacheableQuery)
            {
                var cached = await _cacheService.GetAsync<Result<TResponse>>(cacheableQuery.CacheKey);
                if (cached != null)
                    return cached;
            }


            var handlerType = typeof(IQueryHandler<,>)
                .MakeGenericType(query.GetType(), typeof(TResponse));

            var handler = _serviceProvider.GetService(handlerType);
            if (handler == null)
                return await Result<TResponse>.FaildAsync(false, $"No handler found for query {query.GetType().Name}");

            var method = handlerType.GetMethod("Handle");
            var result = await (Task<Result<TResponse>>)method.Invoke(handler, new object[] { query, cancellationToken });

            if (query is ICacheableQuery<TResponse> cacheable && result.IsSuccess)
            {
                await _cacheService.SetAsync(cacheable.CacheKey, result, cacheable.CacheDuration);
            }

            return result;
        }
    }
}
