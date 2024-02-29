using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

public sealed class Location : ValueObject
{

    public static readonly Location MinLocation = new Location(1,1);
    public static readonly Location MaxLocation = new Location(10,10);

	public int X { get; }
	public int Y { get; }

	protected Location() {}

	protected Location(int x, int y)
	{
		X = x;
		Y = y;
	}

	public static Result<Location, Error> Create(int x, int y)
	{
		if(x < MinLocation.X || x > MaxLocation.X) 
			return GeneralErrors.ValueIsOutOfRange(nameof(x), x, MinLocation.X, MaxLocation.X);

		if(y < MinLocation.Y || y > MaxLocation.Y) 
			return GeneralErrors.ValueIsOutOfRange(nameof(y), y, MinLocation.Y, MaxLocation.Y);

		return new Location(x,y);
	
	}

	public int Distance(Location l)	
	{
		return Math.Abs(l.X - X) + Math.Abs(l.Y - Y);
	}

    public static int operator - (Location first, Location second)
    {
        return first.Distance(second);
    }


    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}
