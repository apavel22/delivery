using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using CSharpFunctionalExtensions;

using Primitives;

namespace DeliveryApp.Core.Domain.OrderAggregate;

/// <summary>
/// Заказ
/// </summary>
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
        public static Error OrderCantBeAssignedToSuchCourierByCapacity()
        {
            return new($"{nameof(Order).ToLowerInvariant()}.cant.be.assigned.to.courier.by.capacity", "Заказ не может быть назначен курьера. Курьер не сможет взять заказ из-за веса");
        }

        public static Error OrderIsNotAssignedToCourier()
        {
            return new($"{nameof(Order).ToLowerInvariant()}.is.not.assigned.to.courier", "Заказ не был назначен на курьера");
        }

    }


    public virtual Guid? CourierId { get; protected set; }
	public virtual Status Status  { get; protected set; }
	public virtual Location Location { get; protected set; }
	public virtual Weight Weight { get; protected set; }

	protected Order() {}

	/// <summary>
	/// ctor:
	/// </summary>
	protected Order(Guid id, Location location, Weight weight, Status status)
	{
		Id = id;
		Location = location;
		Weight = weight;
		CourierId = null;
		Status = status;
	}

	/// <summary>
	/// Создать заказ (вес и location)
	/// - def status: Created
	/// </summary>
	/// <param name="id"></param>
	/// <param name="location"></param>
	/// <param name="weight"></param>
	/// <returns></returns>
	public static Result<Order, Error> Create(Guid id, Location location, Weight weight)
	{
    	if (id == Guid.Empty) return GeneralErrors.ValueIsInvalid(nameof(id));
		if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));
		if (weight == null) return GeneralErrors.ValueIsRequired(nameof(weight));


    	// todo: location == null, weight == null


		return new Order(id, location, weight, Status.Created);
	}

	public Result<object, Error> Assigne(Courier courier)
	{
		if(Status == Status.Completed) return Errors.OrderHasAlreadyCompleted();   	
		if(Status == Status.Assigned) return Errors.OrderHasAlreadyAssigned();   	

		if(!courier.Transport.CanCarry(Weight))
			return Errors.OrderCantBeAssignedToSuchCourierByCapacity();

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
