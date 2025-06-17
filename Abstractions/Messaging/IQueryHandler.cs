using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Abstractions.Messaging
{
    public interface IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
