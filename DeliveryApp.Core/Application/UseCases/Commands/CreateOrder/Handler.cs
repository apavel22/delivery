using MediatR;

using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Ports;

namespace DeliveryApp.Core.Application.UseCases.Commands.CreateOrder;

public class Handler : IRequestHandler<Command, bool>
{
    private readonly IOrderRepository _orderRepository;

    public Handler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<bool> Handle(Command message, CancellationToken cancellationToken)
    {
    	var location = Location.Create(message.X, message.Y);
    	var weight = Weight.Create(message.Weight);

    	// wrong params
    	if(location.IsFailure)	return false;
    	if(weight.IsFailure)   	return false;

    	// order already exists
    	Order existingOrder = await _orderRepository.GetByIdAsync(message.Id);
    	if(existingOrder != null) {
    		Console.WriteLine("here " + message.Id + " " + existingOrder.Id + " " + existingOrder.Weight.Value);
    	   	return false;
    	}

    	// wrogn params
        var order = Order.Create(message.Id, location.Value, weight.Value);
        if (order.IsFailure) {
	        return false;
        }

        _orderRepository.Add(order.Value);

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}