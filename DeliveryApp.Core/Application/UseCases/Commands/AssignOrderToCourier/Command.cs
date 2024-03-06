using MediatR;

using DeliveryApp.Core.Domain.SharedKernel;


namespace DeliveryApp.Core.Application.UseCases.Commands.AssignCourierToOrder;

public class Command : IRequest<bool>
{
	public Guid CourierId { get; private set; }
	public Guid OrderId { get; private set; }

	protected Command()
	{
	}

	public Command(Guid courierId, Guid orderId)
	{
		CourierId = courierId;
		OrderId = orderId;
	}
}