using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

public sealed class Location : ValueObject
{
	public int X { get; }
	public int Y { get; }

	private Location() {}

	public Location(int x, int y)
	{
		if(x < 0 || x > 10) throw new ArgumentOutOfRangeException(x.ToString());
		if(y < 0 || y > 10) throw new ArgumentOutOfRangeException(y.ToString());

		X = x;
		Y = y;
	}

	public int Distance(Location l)	
	{
		return Math.Abs(l.X - X) + Math.Abs(l.Y - Y);
	}


    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }

	public override bool Equals(object obj)
	{
    	var l = obj as Location;
	    if (l == null) 	return false;

	    return (l.X == X && l.Y == Y);
	}
}
