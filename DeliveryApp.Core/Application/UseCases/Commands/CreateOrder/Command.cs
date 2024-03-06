using MediatR;

using DeliveryApp.Core.Domain.SharedKernel;


namespace DeliveryApp.Core.Application.UseCases.Commands.CreateOrder;

public class Command : IRequest<bool>
{
	public Guid Id { get; private set; }
	public int X { get; private set; }
	public int Y { get; private set; }
	public int Weight { get; private set; }

	protected Command()
	{
	}


	public Command(Guid orderId, int x, int y, int weight)
	{
//		if (orderId == Guid.Empty) throw new ArgumentException(nameof(orderId));

		Id = orderId;
		Weight = weight;
		X = x;
		Y = y;
	}

}