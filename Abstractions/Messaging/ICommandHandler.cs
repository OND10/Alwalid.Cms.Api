using Alwalid.Cms.Api.Common.Handler;
namespace Alwalid.Cms.Api.Abstractions.Messaging
{
    public interface ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
