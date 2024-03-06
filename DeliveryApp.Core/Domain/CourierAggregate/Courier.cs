using DeliveryApp.Core.Domain.SharedKernel;
using CSharpFunctionalExtensions;

using Primitives;

namespace DeliveryApp.Core.Domain.CourierAggregate;

/// <summary>
/// Курьер
/// </summary>
public class Courier : Aggregate
{
	public static class Errors
    {
		public static Error CourierHasAlreadyStarted()
        {
            return new($"{nameof(Courier).ToLowerInvariant()}.has.already.started", "Курьер уже работает");
        }

		public static Error CourierHasInvalidStatusToStartWork(Status status)
        {
            return new($"{nameof(Courier).ToLowerInvariant()}.has.invalid.status.to.start.work", "Курьер не может начинать работать из Status {status}");
        }

		public static Error CourierHasAlreadyStopped()
        {
            return new($"{nameof(Courier).ToLowerInvariant()}.has.already.stopped", "Курьер уже не работает");
        }

		public static Error CourierHasInvalidStatusToStopWork(Status status)
        {
            return new($"{nameof(Courier).ToLowerInvariant()}.has.invalid.status.to.stop.work", "Курьер не может заканчивать работать из Status {status}");
        }

		public static Error CourierHasAlreadyInWork()
        {
            return new($"{nameof(Courier).ToLowerInvariant()}.has.already.busy", "Курьер уже выполняет заказ");
        }

		public static Error CourierHasInvalidStatusToInWork(Status status)
        {
            return new($"{nameof(Courier).ToLowerInvariant()}.has.invalid.status.to.assign.to.order", "Курьер не может брать заказ из Status {status}");
        }

    }

    public virtual string Name { get; protected set; }
    public virtual Transport Transport { get; protected set; }
    public virtual Location Location { get; protected set; }
    public virtual Status Status { get; protected set; }

    protected Courier() {}

	/// <summary>
	/// ctor:
	/// </summary>
    protected Courier(Guid id, string name, Transport transport, Location location, Status status) 
    {
    	Id = id;
    	Name = name;
    	Transport = transport;
    	Status = status;
    	Location = location;
    }

	/// <summary>
	/// Создать курьера
	/// </summary>
	/// <remarks>
	/// - def location: Location.MinLocation
	/// - def status NA
	/// </remarks>
	/// <param name="name">Имя</param>
	/// <param name="transport">Транспорт</param>
	/// <returns></returns>
    public static Result<Courier, Error> Create(string name, Transport transport)
    {
		if(transport == null) return GeneralErrors.ValueIsInvalid(nameof(transport));
    	if (string.IsNullOrEmpty(name)) return GeneralErrors.ValueIsInvalid(nameof(name));

    	var id = Guid.NewGuid();
//		var defaultLocation = Location.Create(1,1).Value;
		var defaultLocation = Location.Create(Location.MinLocation).Value;
//		var defaultLocation = Location.MinLocation;
		var defaultStatus = Status.NotAvailable;

    	return new Courier(id, name, transport, defaultLocation, defaultStatus);
    }


	/// <summary>
	/// Start to work:
	/// can start from NA status only
	/// </summary>
	/// <returns></returns>
	public Result<object, Error> StartWork()
    {
		 if (Status == Status.Ready) return Errors.CourierHasAlreadyStarted();   	
		 if (Status != Status.NotAvailable) return Errors.CourierHasInvalidStatusToStartWork(Status);   	

		 Status = Status.Ready;

		 return new object();
    }

	/// <summary>
	/// Stop work
	/// can stop from Ready only
	/// </summary>
	/// <returns></returns>
    public Result<object, Error> StopWork()
    {
		 if (Status == Status.NotAvailable) return Errors.CourierHasAlreadyStopped();   	
		 if (Status != Status.Ready) return Errors.CourierHasInvalidStatusToStopWork(Status);   	

		 Status = Status.NotAvailable;

		 return new object();
    }

	/// <summary>
	/// Stop work
	/// can stop from Ready only
	/// </summary>
	/// <returns></returns>
    public Result<object, Error> CompleteWork()
    {
		 if (Status == Status.Ready) return Errors.CourierHasAlreadyStarted();   	
		 if (Status != Status.InWork) return Errors.CourierHasInvalidStatusToStartWork(Status);   	

		 Status = Status.Ready;

		 return new object();
    }


	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
    public Result<object, Error> InWork()
    {
		 if (Status == Status.InWork) return Errors.CourierHasAlreadyInWork();   	
		 if (Status != Status.Ready) return Errors.CourierHasInvalidStatusToInWork(Status);   	

		 Status = Status.InWork;

		 return new object();
    }

	/// <summary>
	/// За сколько "времени/шагов" можно переместитья в location?
	/// </summary>
	/// <remarks>
	/// - время зависит от скорости транспорта
	/// - время целое, но с запасом: 8/3 = 3 шага, а не 2 если делить нацело
	/// - distance <= time* speed
	/// </remarks>
	/// <param name="location"></param>
	/// <returns></returns>
    public Result<int, Error> CalculateTimeToPoint(Location location)
    {
		if(location == null) return GeneralErrors.ValueIsRequired(nameof(location));

    	int distance = (Location - location).Value;

		// уже на месте?
    	if(distance == 0) return 0;

    	int time = distance / Transport.Speed.Value;

    	// с запасом: time+1 если не делится нацело
    	if( time * Transport.Speed.Value < distance) time++;

    	return time;
	}

	/// <summary>
	/// Переместиться в сторону location
	/// </summary>
	/// <remarks>
	/// - может за один раз не добраться до location
	/// - всего нужно CalculateTimeToPoint чтобы попасть в location
	/// - учитывается скорость транспорта
	/// - сначала перемещается по X, потом по Y
	/// </remarks>
	/// <example>
	/// </example>
	/// <param name="locationTo"></param>
	/// <returns></returns>
    public Result<object, Error> Move(Location locationTo)
    {
		if(locationTo == null) return GeneralErrors.ValueIsRequired(nameof(locationTo));

    	var distance_x = Math.Abs(Location.X - locationTo.X);
    	var distance_y = Math.Abs(Location.Y - locationTo.Y);

    	// уже на месте
    	if(distance_x == 0 && distance_y == 0) return new object();

    	// на сколько можно сдвинуться за 1 шаг
    	int delta = Transport.Speed.Value;
    	int remain = 0;

   	    int new_x = 0;
   	    int new_y = 0;

   	    // на сколько можно сдвинуться по X: 
   	    // или на distance_x или на speed, смотря что меньше
   	    int delta_x = Math.Min(distance_x, Transport.Speed.Value);

   	    // остаток от движения по X
   	    remain = Transport.Speed.Value - distance_x;
   	    if(remain <0) remain = 0;

   	    // по Y можно сдвинуться или на остаток или на distance_y, если оно меньше остатка
   	    int delta_y = Math.Min(distance_y, remain);

   	    // сдвигаемся в зависимости от направления
   	    if(Location.X > locationTo.X) new_x = Location.X - delta_x;	
  	    else new_x = Location.X + delta_x;

   	    if(Location.Y > locationTo.Y) new_y = Location.Y - delta_y;	
  	    else new_y = Location.Y + delta_y;

    	Location = Location.Create(new_x, new_y).Value;

    	return new object();
	}
}
