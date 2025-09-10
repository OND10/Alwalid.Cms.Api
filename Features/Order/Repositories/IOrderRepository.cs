using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Order.Repositories
{
	public interface IOrderRepository
	{
	
		Task<Alwalid.Cms.Api.Entities.Order?>GetAsync(Guid id, CancellationToken cancellationToken = default);
		Task AddAsync(Cms.Api.Entities.Order order, CancellationToken cancellationToken = default);
		Task UpdateAsync(Cms.Api.Entities.Order order, CancellationToken cancellationToken = default);
	}
}
