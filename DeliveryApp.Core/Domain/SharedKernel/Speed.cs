using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

public sealed class Speed : ValueObject
{
    public static readonly Speed MinSpeed = new Speed(1);
    public static readonly Speed MaxSpeed = new Speed(10);


	public int Value { get; }

	protected Speed() {}

	protected Speed(int value)
	{
	    Value = value;
	}

	public static Result<Speed, Error> Create(int value)
	{
	    if(value < MinSpeed.Value) 
			return GeneralErrors.ValueIsLowerThan(nameof(value), value, MinSpeed.Value);

	    if(value > MaxSpeed.Value) 
			return GeneralErrors.ValueIsGreaterThan(nameof(value), value, MaxSpeed.Value);

		return new Speed(value);
	}


    public static bool operator < (Speed first, Speed second)
    {
        return first.Value < second.Value;
    }

    public static bool operator > (Speed first, Speed second)
    {
        return first.Value > second.Value;
    }

    public static bool operator <= (Speed first, Speed second)
    {
        return first.Value <= second.Value;
    }

    public static bool operator >= (Speed first, Speed second)
    {
        return first.Value >= second.Value;
    }


    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
