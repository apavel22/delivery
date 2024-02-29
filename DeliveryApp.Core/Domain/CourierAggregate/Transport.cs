using DeliveryApp.Core.Domain.SharedKernel;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.CourierAggregate;

public class Transport : Entity<int>
{
    public static readonly Transport Pedestrian  = new(1, nameof(Pedestrian).ToLowerInvariant(), Speed.Create(1).Value, Weight.Create(1).Value);
    public static readonly Transport Bicycle   = new(2, nameof(Bicycle).ToLowerInvariant(), Speed.Create(2).Value, Weight.Create(4).Value);
    public static readonly Transport Scooter    = new(3, nameof(Scooter).ToLowerInvariant(), Speed.Create(3).Value, Weight.Create(6).Value);
    public static readonly Transport Car     = new(4, nameof(Car).ToLowerInvariant(), Speed.Create(4).Value, Weight.Create(8).Value);


    public string Name { get; protected set; }
    
    public Speed Speed { get; protected set; }

    public Weight Capacity { get; protected set; }


    public static class Errors
    {
		public static Error TransportIsWrong(int id)
        {
            return new($"{nameof(Transport).ToLowerInvariant()}.is.wrong", 
                $"Не верное значение {id}. Допустимые значения: { nameof(Transport).ToLowerInvariant()}: { string.Join(",", List().Select(s => s.Id))}");
        }
		public static Error TransportIsWrong(string name)
        {
            return new($"{nameof(Transport).ToLowerInvariant()}.is.wrong", 
                $"Не верное значение {name}. Допустимые значения: { nameof(Transport).ToLowerInvariant()}: { string.Join(",", List().Select(s => s.Name))}");
        }
    }

	protected Transport() {}

	protected Transport(int id, string name, Speed speed, Weight capacity) : base(id)
	{
	    Name = name;
	    Speed = speed;
	    Capacity = capacity;
	}


    public bool CanWeight(Weight weight) 
    {
    	return weight <= Capacity;
    }

	public static IEnumerable<Transport> List()
    {
        yield return Pedestrian;
        yield return Bicycle;
        yield return Scooter;
        yield return Car ;
    }

	public static Result<Transport, Error> FromName(string name)
    {
        var transport = List()
            .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (transport == null) return Errors.TransportIsWrong(name);
        return transport;
    }

    public static Result<Transport, Error> From(int id)
    {
        var transport = List().SingleOrDefault(s => s.Id == id);
        if (transport == null) return Errors.TransportIsWrong(id);
        return transport;
    }
}

