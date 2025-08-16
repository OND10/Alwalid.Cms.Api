using Alwalid.Cms.Api.Abstractions;
using Alwalid.Cms.Api.Features.ProductImage.Queries.GetProductImageById;

namespace Alwalid.Cms.Api.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        // It works same as dispatcher
        public EventPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var handlerInterface = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = _serviceProvider.GetServices(handlerInterface);

            // Invoke each handler’s HandleAsync
            var handleMethod = handlerInterface.GetMethod("HandleAsync")!;

            foreach (var handler in handlers)
            {
                var task = (Task)handleMethod.Invoke(handler, new object?[] { domainEvent, cancellationToken })!;
                await task.ConfigureAwait(false);
            }
        }

        public async Task PublishAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
        {
            foreach(var domainEvent in domainEvents)
            {
                await PublishAsync(domainEvent, cancellationToken);
            }
        }
    }
}
