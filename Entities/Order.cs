namespace Alwalid.Cms.Api.Entities
{

	public enum OrderState
	{
		Created = 0,
		Paid = 1,
		Packed = 2,
		Shipped = 3,
		Delivered = 4,
		Cancelled = 5
	}
	public enum OrderTrigger
	{
		Pay,
		Pack,
		Ship,
		Deliver,
		Cancel
	}

	public sealed class Order
	{
		public Guid Id { get; init; } = Guid.NewGuid();
		public string CustomerEmail { get; set; } = string.Empty;
		public OrderState State { get; private set; } = OrderState.Created;
		public DateTimeOffset CreatedAt { get; init; } = DateTime.UtcNow;
		public decimal Amount { get; init; }
		public string? TrackingNumber { get; private set; }

		internal void SetState(OrderState newState) => State = newState;
		internal void SetTracking(string tracking) => TrackingNumber = tracking;
	}
}
