using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

public sealed class Weight : ValueObject
{
	public int Value { get; }

	protected Weight() {}

	protected Weight(int value)
	{
	    Value = value;
	}

	public static Result<Weight, Error> Create(int value)
	{
	    if(value <= 0) 
			return GeneralErrors.ValueIsLowerThan(nameof(value), value, 0);

		return new Weight(value);
	}


    public static bool operator < (Weight first, Weight second)
    {
        return first.Value < second.Value;
    }

    public static bool operator > (Weight first, Weight second)
    {
        return first.Value > second.Value;
    }

	public override bool Equals(object obj)
	{
    	var w = obj as Weight;
	    if (w == null) 	return false;

	    return (w.Value == Value);
	}


    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
