using DeliveryApp.Core.Domain.CourierAggregate;
using Primitives;

namespace DeliveryApp.Core.Ports;

/// <summary>
/// Репозиторий для Курьера
/// </summary>
public interface ICourierRepository : IRepository<Courier>
{

    /// <summary>
    /// Добавить курьера
    /// </summary>
    /// <param name="courier"></param>
    /// <returns></returns>
    Courier Add(Courier courier);

    /// <summary>
    /// Обновить курьера
    /// </summary>
    /// <param name="courier"></param>
    void Update(Courier courier);

    /// <summary>
    /// Получить курьера по Id
    /// </summary>
    /// <param name="courierId"></param>
    /// <returns></returns>
    Task<Courier> GetByIdAsync(Guid courierId); 

    /// <summary>
    /// Получить всех Ready
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Courier>> GetAllReadyAsync();
}

