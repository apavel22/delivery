using MediatR;

using DeliveryApp.Core.Domain.SharedKernel;


namespace DeliveryApp.Core.Application.UseCases.Commands.CreateOrder;

public class Command : IRequest<bool>
{
	public Guid Id { get; set; }
	public Location Location { get; set; }
	public Weight Weight { get; set; }

	protected Command()
	{
	}


	public Command(Guid orderId, Location location, Weight weight)
	{
		if (orderId == Guid.Empty) throw new ArgumentException(nameof(orderId));
		if (location == null) throw new ArgumentException(nameof(location));
		if (weight == null) throw new ArgumentException(nameof(weight));

		Id = orderId;
		Location = location;
		Weight = weight;
	}

}