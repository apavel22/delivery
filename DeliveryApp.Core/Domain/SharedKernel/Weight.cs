using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

/// <summary>
/// Вес
/// </summary>
/// <remarks>
/// - MinWeight <= weight
/// - можно сравнивать больше-меньше
/// </remarks>
public sealed class Weight : ValueObject
{
    public static readonly Weight MinWeight = new Weight(1);


	public int Value { get; }

	protected Weight() {}

    /// <summary>
    /// ctor:
    /// </summary>
	protected Weight(int value) : this()
	{
	    Value = value;
	}

    /// <summary>
    /// Создать вес
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
	public static Result<Weight, Error> Create(int value)
	{
	    if(value < MinWeight.Value) 
			return GeneralErrors.ValueIsLowerThan(nameof(value), value, MinWeight.Value);

		return new Weight(value);
	}


    public static bool operator < (Weight first, Weight second)
    {
        return first.Value < second.Value;
    }

    public static bool operator <= (Weight first, Weight second)
    {
        return first.Value <= second.Value;
    }


    public static bool operator > (Weight first, Weight second)
    {
        return first.Value > second.Value;
    }

    public static bool operator >= (Weight first, Weight second)
    {
        return first.Value >= second.Value;
    }


    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
