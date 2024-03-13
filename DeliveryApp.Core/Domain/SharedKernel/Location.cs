using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

/// <summary>
/// Location (x,y)
/// - MinLocation <= (x,y) <= MaxLocation()
/// </summary>
public sealed class Location : ValueObject
{

	public static readonly Location MinLocation = new Location(1, 1);
	public static readonly Location MaxLocation = new Location(10, 10);

	public int X { get; }
	public int Y { get; }

	protected Location() { }

	/// <summary>
	/// ctor:
	/// </summary>
	protected Location(int x, int y) : this()
	{
		X = x;
		Y = y;
	}


	/// <summary>
	/// Create location from another location
	/// </summary>
	public static Result<Location, Error> Create(Location location)
	{
		if (location == null)
			return GeneralErrors.ValueIsRequired(nameof(location));

		return Create(location.X, location.Y);
	}

	/// <summary>
	/// Create location
	/// </summary>
	/// <remarks>
	/// - MinLocation <= (x,y) <= MaxLocation()
	/// </remarks>
	/// <returns></returns>
	public static Result<Location, Error> Create(int x, int y)
	{
		if (x < MinLocation.X || x > MaxLocation.X)
			return GeneralErrors.ValueIsOutOfRange(nameof(x), x, MinLocation.X, MaxLocation.X);

		if (y < MinLocation.Y || y > MaxLocation.Y)
			return GeneralErrors.ValueIsOutOfRange(nameof(y), y, MinLocation.Y, MaxLocation.Y);

		return new Location(x, y);
	}

	/// <summary>
	/// Расстояние до другой точки
	/// </summary>
	/// <param name="location"></param>
	/// <returns></returns>
	public Result<int, Error> Distance(Location location)
	{
		if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));
		return Math.Abs(location.X - X) + Math.Abs(location.Y - Y);
	}

	/// <summary>
	///  Расстояние между точками
	/// </summary>
	/// <param name="first"></param>
	/// <param name="second"></param>
	/// <returns></returns>
	public static Result<int, Error> operator -(Location first, Location second)
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
