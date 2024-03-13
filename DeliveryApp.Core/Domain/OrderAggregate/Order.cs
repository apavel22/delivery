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
		public static Error OrderHasNotBeAssignedToCourier()
		{
			return new($"{nameof(Order).ToLowerInvariant()}.is.not.assigned.to.courier", "Заказ не был назначен на курьера");
		}

		/*
				public static Error OrderCantBeAssignedToNonReadyCourier()
				{
					return new($"{nameof(Order).ToLowerInvariant()}.cant.be.assigned.to.non.ready.courier", "Заказ не может быть назначен курьера. Курьер не готов взять заказ");
				}
				public static Error OrderCantBeAssignedToSuchCourierByCapacity()
				{
					return new($"{nameof(Order).ToLowerInvariant()}.cant.be.assigned.to.courier.by.capacity", "Заказ не может быть назначен курьера. Курьер не сможет взять заказ из-за веса");
				}
		*/

	}

	// Data:
	public virtual Weight Weight { get; protected set; }
	public virtual Location Location { get; protected set; }
	public virtual Status Status { get; protected set; }
	public virtual Guid? CourierId { get; protected set; }

	protected Order() { }

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

		return new Order(id, location, weight, Status.New);
	}

	public Result<object, Error> AssignToCourier(Courier courier)
	{
		if (Status == Status.Assigned) return Errors.OrderHasAlreadyAssigned();
		if (Status == Status.Completed) return Errors.OrderHasAlreadyCompleted();

		//считаем, что целостность всей модели ПО соблюдена где-то в другом месте
		/*
			if(!courier.Transport.CanCarry(Weight).IsSuccess)
				return Errors.OrderCantBeAssignedToSuchCourierByCapacity();

			if(courier.Status != CourierAggregate.Status.Ready)
				return Errors.OrderCantBeAssignedToNonReadyCourier();   	
		*/

		CourierId = courier.Id;

		Status = Status.Assigned;

		return new object();
	}

	public Result<object, Error> Complete()
	{
		if (Status == Status.Completed) return Errors.OrderHasAlreadyCompleted();
		if (Status != Status.Assigned) return Errors.OrderHasNotBeAssignedToCourier();

		Status = Status.Completed;

		return new object();
	}
}
