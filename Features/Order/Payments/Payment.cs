namespace Alwalid.Cms.Api.Features.Order.Payments
{
	public interface IPaymentProbe
	{
		bool HasPayment(Guid orderId);
	}

	// super fake payment checker: “paid” if Amount’s first hex char of Id is even (just for demo)
	// Replace with a real payment lookup and store the result in your DB.
	public sealed class FakePaymentProbe : IPaymentProbe
	{
		public bool HasPayment(Guid orderId)
		{
			// Deterministic but arbitrary demo condition
			var first = orderId.ToString("N")[0];
			return "02468ace".Contains(char.ToLowerInvariant(first));
		}
	}
}
