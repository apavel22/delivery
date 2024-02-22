using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

public sealed class Weight : ValueObject
{
	public int Value { get; }

	private Weight() {}

	public Weight(int value)
	{
	    if(value <= 0) throw new ArgumentOutOfRangeException(value.ToString());

	    Value = value;
	}

    public static bool operator <(Weight weight1, Weight weight2)
    {
        return weight1.Value < weight2.Value;
    }

    public static bool operator >(Weight weight1, Weight weight2)
    {
        return weight1.Value > weight2.Value;
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