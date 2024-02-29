using DeliveryApp.Core.Domain.SharedKernel;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.CourierAggregate;

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

		public static Error CourierHasAlreadyBusy()
        {
            return new($"{nameof(Courier).ToLowerInvariant()}.has.already.busy", "Курьер уже выполняет заказ");
        }

		public static Error CourierHasInvalidStatusToAssignToOrder(Status status)
        {
            return new($"{nameof(Courier).ToLowerInvariant()}.has.invalid.status.to.assign.to.order", "Курьер не может брать заказ из Status {status}");
        }


    }

    public virtual string Name { get; protected set; }
    public virtual Transport Transport { get; protected set; }
    public virtual Location Location { get; protected set; }
    public virtual Status Status { get; protected set; }

    protected Courier() {}

    protected Courier(Guid id, string name, Transport transport, Location location, Status status) 
    {
    	Id = id;
    	Name = name;
    	Transport = transport;
    	Status = status;
    	Location = location;
    }

    public static Result<Courier, Error> Create(string name, Transport transport)
    {
    	var id = Guid.NewGuid();
    	if (string.IsNullOrEmpty(name)) return GeneralErrors.ValueIsInvalid(nameof(name));

    	return new Courier(id, name, transport, Location.Create(1,1).Value, Status.NotAvailable);
    }


    public Result<object, Error> StartWork()
    {
		 if (Status == Status.Ready) return Errors.CourierHasAlreadyStarted();   	
		 if (Status != Status.NotAvailable) return Errors.CourierHasInvalidStatusToStartWork(Status);   	

		 Status = Status.Ready;

		 return new object();
    }

    public Result<object, Error> StopWork()
    {
		 if (Status == Status.NotAvailable) return Errors.CourierHasAlreadyStopped();   	
		 if (Status != Status.Ready) return Errors.CourierHasInvalidStatusToStopWork(Status);   	

		 Status = Status.NotAvailable;

		 return new object();
    }


    public Result<object, Error> InWork()
    {
		 if (Status == Status.Busy) return Errors.CourierHasAlreadyBusy();   	
		 if (Status != Status.Ready) return Errors.CourierHasInvalidStatusToAssignToOrder(Status);   	

		 Status = Status.Busy;

		 return new object();
    }

    public Result<int, Error> CalculateTimeToPoint(Location location)
    {
    	int distance = Location - location;
    	if(distance == 0) return 0;

    	int time = distance / Transport.Speed.Value;

    	// norm +1 for fractional
    	if( time * Transport.Speed.Value < distance) time++;

    	return time;
	}

    public Result<object, Error> Move(Location to)
    {
    	var distance_x = Math.Abs(Location.X - to.X);
    	var distance_y = Math.Abs(Location.Y - to.Y);

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
   	    if(Location.X > to.X) new_x = Location.X - delta_x;	
  	    else new_x = Location.X + delta_x;

   	    if(Location.Y > to.Y) new_y = Location.Y - delta_y;	
  	    else new_y = Location.Y + delta_y;

    	Location = Location.Create(new_x, new_y).Value;

    	return new object();
	}

}
