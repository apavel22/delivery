using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

public sealed class Location : ValueObject
{
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
		if(x < 1 || x > 10) 
			return GeneralErrors.ValueIsOutOfRange(nameof(x), x, 1, 10);

		if(y < 1 || y > 10) 
			return GeneralErrors.ValueIsOutOfRange(nameof(y), y, 1, 10);

		return new Location(x,y);
	
	}

	public int Distance(Location l)	
	{
		return Math.Abs(l.X - X) + Math.Abs(l.Y - Y);
	}


	public override bool Equals(object obj)
	{
    	var l = obj as Location;
	    if (l == null) 	return false;

	    return (l.X == X && l.Y == Y);
	}

    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}
