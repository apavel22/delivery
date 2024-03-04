using DeliveryApp.Core.Domain.OrderAggregate;
using Primitives;

namespace DeliveryApp.Core.Ports;

/// <summary>
/// Репозиторий для Order
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// Добавить заказ
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Order Add(Order order);

    /// <summary>
    /// обновить заказ
    /// </summary>
    /// <param name="order"></param>
    void Update(Order order);

    /// <summary>
    /// Получить заказ по Id
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<Order> GetByIdAsync(Guid orderId); 

    /// <summary>
    /// Получить все неаспределенные заказы
    /// </summary>
    /// <returns></returns>
    IEnumerable<Order> GetAllUnassigned();
}

