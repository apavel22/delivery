using MediatR;

using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Ports;

namespace DeliveryApp.Core.Application.UseCases.Commands.CourierStartWork;

public class Handler : IRequestHandler<Command, bool>
{
    private readonly ICourierRepository _courierRepository;

    public Handler(ICourierRepository courierRepository)
    {
        _courierRepository = courierRepository ?? throw new ArgumentNullException(nameof(courierRepository));
    }

    public async Task<bool> Handle(Command message, CancellationToken cancellationToken)
    {
    	var courier = await _courierRepository.GetByIdAsync(message.CourierId);
    	if(courier == null) return false;

    	if(courier.Status != Status.Ready) return false;

    	// wrong params
        var result = courier.StartWork();
        if (result.IsFailure)  return false;

        _courierRepository.Update(courier);

        return await _courierRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
