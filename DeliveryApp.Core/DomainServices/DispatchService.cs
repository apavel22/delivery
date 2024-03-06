using CSharpFunctionalExtensions;
using Primitives;

using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.CourierAggregate;

namespace DeliveryApp.Core.DomainServices;

public static class DispatchService
{
	public static Courier Dispatch(Order order, IEnumerable<Courier> couriers)
	{
		if(order.Status != Domain.OrderAggregate.Status.New) return null;

		return couriers
				.Where(c => c.Status == Domain.CourierAggregate.Status.Ready)
				.Where(c => c.CanCarry(order.Weight).Value == true)
				.OrderBy(b=>b.CalculateTimeToPoint(order.Location).Value)
				.First();
	}
}
