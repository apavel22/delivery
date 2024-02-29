using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

public sealed class Weight : ValueObject
{
    public static readonly Weight MinWeight = new Weight(1);


	public int Value { get; }

	protected Weight() {}

	protected Weight(int value)
	{
	    Value = value;
	}

	public static Result<Weight, Error> Create(int value)
	{
	    if(value < MinWeight.Value) 
			return GeneralErrors.ValueIsLowerThan(nameof(value), value, MinWeight.Value);

		return new Weight(value);
	}


    public static bool operator <= (Weight first, Weight second)
    {
        return first.Value <= second.Value;
    }


    public static bool operator >= (Weight first, Weight second)
    {
        return first.Value >= second.Value;
    }

    public static bool operator < (Weight first, Weight second)
    {
        return first.Value < second.Value;
    }

    public static bool operator > (Weight first, Weight second)
    {
        return first.Value > second.Value;
    }

    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
