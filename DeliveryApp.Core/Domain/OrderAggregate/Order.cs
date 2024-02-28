using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using CSharpFunctionalExtensions;

using Primitives;

namespace DeliveryApp.Core.Domain.OrderAggregate;

public class Order : Aggregate
{
    public static class Errors
    {
        public static Error OrderHasAlreadyAssigned()
        {
            return new($"{nameof(Order).ToLowerInvariant()}.has.already.assigned.to.courier", "Заказ уже назначен на курьера");
        }
        public static Error OrderHasAlreadyCompleted()
        {
            return new($"{nameof(Order).ToLowerInvariant()}.has.already.completed", "Заказ уже выполнен");
        }
        public static Error OrderCantBeAssignedToNonReadyCourier()
        {
            return new($"{nameof(Order).ToLowerInvariant()}.cant.be.assigned.to.non.ready.courier", "Заказ не может быть назначен курьера. Курьер не готов взять заказ");
        }
        public static Error OrderIsNotAssignedToCourier()
        {
            return new($"{nameof(Order).ToLowerInvariant()}.is.not.assogned.to.cuorier", "Заказ не был назначен на курьера");
        }

    }


    public virtual Guid CourierId { get; protected set; }
	public virtual Status Status  { get; protected set; }
	public virtual Location Location { get; protected set; }
	public virtual Weight Weight { get; protected set; }

	protected Order() {}
	protected Order(Guid id, Location location, Weight weight)
	{
		Id = id;
		Location = location;
		Weight = weight;
		CourierId = Guid.Empty;
		Status = Status.Created;
	}

	public static Result<Order, Error> Create(Guid id, Location location, Weight weight)
	{
    	if (id == Guid.Empty) return GeneralErrors.ValueIsInvalid(nameof(id));

		return new Order(id, location, weight);
	}

	public Result<object, Error> Assigne(Courier courier)
	{
		if(Status == Status.Completed) return Errors.OrderHasAlreadyCompleted();   	
		if(Status == Status.Assigned) return Errors.OrderHasAlreadyAssigned();   	

		if(courier.InWork().IsSuccess == false)
			return Errors.OrderCantBeAssignedToNonReadyCourier();   	

    	CourierId = courier.Id;

		Status = Status.Assigned;

	    return new object();
	}


	public Result<object, Error> Complete(Courier courier)
	{
		if(Status == Status.Completed) return Errors.OrderHasAlreadyCompleted();   	
		if(Status != Status.Assigned) return Errors.OrderIsNotAssignedToCourier();   	

		Status = Status.Completed;

	    return new object();
	}

}
