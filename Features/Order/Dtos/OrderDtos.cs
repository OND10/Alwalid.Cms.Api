namespace Alwalid.Cms.Api.Features.Order.Dtos
{
	public record CreateOrderDto(string CustomerEmail, decimal Amount);

	public record OrderResponse(Guid Id, string CustomerEmail, decimal Amount, string State, string? TrackingNumber, DateTimeOffset CreatedAt)
	{
		public OrderResponse(Entities.Order o) : this(o.Id, o.CustomerEmail, o.Amount, o.State.ToString(), o.TrackingNumber, o.CreatedAt) { }
	}
}
