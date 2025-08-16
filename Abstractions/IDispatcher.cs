using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;

namespace Alwalid.Cms.Api.Abstractions
{
    public interface IDispatcher
    {
        Task<Result<TResponse>> SendCommandAsync<TResponse>(
            ICommand<TResponse> command,
            CancellationToken cancellationToken = default
        );

        Task<Result<TResponse>> SendQueryAsync<TResponse>(
            IQuery<TResponse> query,
            CancellationToken cancellationToken = default
        );
    }
}
