using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

/// <summary>
/// Скорость
/// - MinSpeed <= (speed) <= MaxSpeed()
/// </summary>
/// <remarks>
/// Можно сравнивать больше-меньше
/// </remarks>
public sealed class Speed : ValueObject
{
    public static readonly Speed MinSpeed = new Speed(1);
    public static readonly Speed MaxSpeed = new Speed(10);


	public int Value { get; }

	protected Speed() {}

    /// <summary>
    /// ctor:
    /// </summary>
	protected Speed(int value) : this()
	{
	    Value = value;
	}

    /// <summary>
    /// Создать скорость
    /// </summary>
    /// <remarks>
    /// - MinSpeed <= (speed) <= MaxSpeed()
    /// </remarks>
    /// <returns></returns>
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

    public static bool operator <= (Speed first, Speed second)
    {
        return first.Value <= second.Value;
    }

    public static bool operator > (Speed first, Speed second)
    {
        return first.Value > second.Value;
    }

    public static bool operator >= (Speed first, Speed second)
    {
        return first.Value >= second.Value;
    }

    /// <summary>
    /// to int
    /// </summary>
    /// <param name="value"></param>
	public static implicit operator int(Speed value)
    {
		return value.Value;
    }

    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
