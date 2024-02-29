using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.OrderAggregate;

public class Status : Entity<int>
{
    public static readonly Status Created = new Status(1, nameof(Created).ToLowerInvariant());
    public static readonly Status Assigned = new Status(2, nameof(Assigned).ToLowerInvariant());
    public static readonly Status Completed = new Status(3, nameof(Completed).ToLowerInvariant());

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

	protected Status(int id, string name)
    {
        Id = id;
        Name = name;
    }

	public static IEnumerable<Status> List()
    {
        yield return Created;
        yield return Assigned;
        yield return Completed;
    }

	public static Result<Status, Error> FromName(string name)
    {
        var state = List()
            .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (state == null) return Errors.StatusIsWrong(name);
        return state;
    }


	public static Result<Status, Error> From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);
        if (state == null) return Errors.StatusIsWrong(id);
        return state;
    }

}
    
