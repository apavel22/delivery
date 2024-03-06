using MediatR;

using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Ports;

namespace DeliveryApp.Core.Application.UseCases.Commands.CourierMoveOneStep;

public class Handler : IRequestHandler<Command, bool>
{
    private readonly ICourierRepository _courierRepository;
    private readonly IOrderRepository _orderRepository;

    public Handler(ICourierRepository courierRepository, IOrderRepository orderRepository)
    {
        _courierRepository = courierRepository ?? throw new ArgumentNullException(nameof(courierRepository));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<bool> Handle(Command message, CancellationToken cancellationToken)
    {
    	var courier = await _courierRepository.GetByIdAsync(message.CourierId);
    	if(courier == null) return false;

    	if(courier.Status != Status.InWork) return false;

    	var order = await _orderRepository.GetAssignedOrderByCourierIdAsync(courier.Id);
    	if(order == null) return false;

    	if(courier.Location.Distance(order.Location).Value != 0)
    	{
	        var result = courier.Move(order.Location);
	        if (result.IsFailure)  return false;
	    }

    	if(courier.Location.Distance(order.Location).Value == 0) 
    	{
    		courier.CompleteWork();
	   		order.Complete();

 	       _orderRepository.Update(order);
    	}

        _courierRepository.Update(courier);

        return await _courierRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
