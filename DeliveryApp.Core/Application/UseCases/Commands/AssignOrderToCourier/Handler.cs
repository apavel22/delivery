using MediatR;

using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Ports;

using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.AssignCourierToOrder;

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

    	if(courier.Status != Domain.CourierAggregate.Status.Ready) return false;

    	var order = await _orderRepository.GetByIdAsync(message.OrderId);
    	if(order == null) return false;

    	if(order.Status != Domain.OrderAggregate.Status.New) return false;

    	var resultCanCarry = courier.CanCarry(order.Weight);
    	if(resultCanCarry.IsFailure) return false;
    	if(resultCanCarry.Value == false) return false;

    	if(order.AssignToCourier(courier).IsFailure) return false;

    	if(courier.InWork().IsFailure) return false;

    	await using var transaction = await _orderRepository.UnitOfWork.BeginTransactionAsync();

        _courierRepository.Update(courier);
        _orderRepository.Update(order);

        await _orderRepository.UnitOfWork.CommitTransactionAsync(transaction);

        return true;
//        return await _courierRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
