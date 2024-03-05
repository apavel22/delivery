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
    	var order = Order.Create(message.Id, message.Location, message.Weight);
    	if(order.IsFailure) return false;

    	_orderRepository.Add(order.Value);

        await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

    	return true;
   	}
}