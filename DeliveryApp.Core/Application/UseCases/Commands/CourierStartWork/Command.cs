using MediatR;

using DeliveryApp.Core.Domain.SharedKernel;


namespace DeliveryApp.Core.Application.UseCases.Commands.CourierStartWork;

public class Command : IRequest<bool>
{
	public Guid CourierId { get; private set; }

	protected Command()
	{
	}

	public Command(Guid courierId)
	{
		CourierId = courierId;
	}
}