using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.StateMachine
{
	public sealed class OrderStateMachine
	{
		private readonly Order _order;
		private readonly StateMachine<OrderState, OrderTrigger> _stateMachine;

		public OrderStateMachine(Order order, Func<bool> paymentCollectedGuard)
		{
			_order = order;
			_stateMachine = new StateMachine<OrderState, OrderTrigger>()
				.Permit(OrderState.Created, OrderState.Paid, OrderTrigger.Pay, guard: paymentCollectedGuard)
				.Permit(OrderState.Created, OrderState.Cancelled, OrderTrigger.Cancel)
				.Permit(OrderState.Paid, OrderState.Packed, OrderTrigger.Pack)
				.Permit(OrderState.Paid, OrderState.Cancelled, OrderTrigger.Cancel)
				.Permit(OrderState.Packed, OrderState.Shipped, OrderTrigger.Ship, onTransition: () =>
				{
					if (string.IsNullOrWhiteSpace(_order.TrackingNumber))
					{
						_order.SetTracking($"TRK-{_order.Id.ToString()[..8].ToUpper()}");
					}
				})
				.Permit(OrderState.Shipped, OrderState.Delivered, OrderTrigger.Deliver);
		}

		public bool CanFire(OrderTrigger trigger)
		{
			return _stateMachine.CanFire(_order.State, trigger);
		}
		public void Fire(OrderTrigger trigger)
		{
			var newState = _stateMachine.Fire(_order.State,trigger);
			_order.SetState(newState);	
		}
		public IEnumerable<(OrderTrigger triggers,  OrderState toStates)> GetPermittedTriggers()
		{
			return _stateMachine.GetPermittedTriggers(_order.State);
		}
	}
}