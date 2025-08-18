using Alwalid.Cms.Api.Abstractions;
using Alwalid.Cms.Api.Features.ProductImage.Queries.GetProductImageById;
using Microsoft.OpenApi.Writers;

namespace Alwalid.Cms.Api.Events
{
    public class EventPublisher : IEventPublisher
    {

        // root provider
        private readonly IServiceProvider _serviceProvider;

        // It works same as dispatcher
        public EventPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            // Create DI scope so scoped services can be resolved
            using var scope = _serviceProvider.CreateScope();

            var handlerInterface = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = scope.ServiceProvider.GetServices(handlerInterface);

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
