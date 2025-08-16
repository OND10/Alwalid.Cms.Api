using Alwalid.Cms.Api.Abstractions;

namespace Alwalid.Cms.Api.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
        Task PublishAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
    }
}
