using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.CourierAggregate;

/// <summary>
/// Статус курьера
/// </summary>
public class Status : Entity<int>
{
    public static readonly Status NotAvailable  = new Status(1, nameof(NotAvailable).ToLowerInvariant());
    public static readonly Status Ready  = new Status(2, nameof(Ready).ToLowerInvariant());
    public static readonly Status Busy  = new Status(3, nameof(Busy ).ToLowerInvariant());

	public static class Errors
    {
        public static Error StatusIsWrong(int id)
        {
            return new($"{nameof(Status).ToLowerInvariant()}.is.wrong", 
                $"Не верное значение {id}. Допустимые значения: {nameof(Status).ToLowerInvariant()}: { string.Join(",", List().Select(s => s.Id))}");
        }
        public static Error StatusIsWrong(string name)
        {
            return new($"{nameof(Status).ToLowerInvariant()}.is.wrong", 
                $"Не верное значение {name}. Допустимые значения: {nameof(Status).ToLowerInvariant()}: { string.Join(",", List().Select(s => s.Name))}");
        }
    }


	public virtual string Name { get; }

	protected Status()  {}

    /// <summary>
    /// ctor:
    /// </summary>
	protected Status(int id, string name)
    {
        Id = id;
        Name = name;
    }

	public static IEnumerable<Status> List()
    {
        yield return NotAvailable;
        yield return Ready;
        yield return Busy;
    }

    /// <summary>
    /// Status by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
	public static Result<Status, Error> FromName(string name)
    {
        var state = List()
            .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (state == null) return Errors.StatusIsWrong(name);
        return state;
    }


    /// <summary>
    /// Status by id
    /// </summary>
    /// <returns></returns>
	public static Result<Status, Error> From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);
        if (state == null) return Errors.StatusIsWrong(id);
        return state;
    }
}
    
