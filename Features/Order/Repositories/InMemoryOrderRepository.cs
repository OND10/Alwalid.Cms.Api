using System.Collections.Concurrent;

namespace Alwalid.Cms.Api.Features.Order.Repositories
{
	public sealed class InMemoryOrderRepository : IOrderRepository
	{
		private readonly ConcurrentDictionary<Guid, Entities.Order> _store = new();

		public Task<Entities.Order?> GetAsync(Guid id, CancellationToken ct = default)
			=> Task.FromResult(_store.TryGetValue(id, out var o) ? o : null);

		public Task AddAsync(Entities.Order order, CancellationToken ct = default)
		{
			_store[order.Id] = order;
			return Task.CompletedTask;
		}

		public Task UpdateAsync(Entities.Order order, CancellationToken ct = default)
		{
			_store[order.Id] = order;
			return Task.CompletedTask;
		}
	}

}
