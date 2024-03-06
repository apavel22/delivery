using System;
using Microsoft.EntityFrameworkCore;

using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Ports;

using Primitives;

namespace DeliveryApp.Infrastructure.Adapters.Postgres;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public IUnitOfWork UnitOfWork => _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public void Add(Order data)
    {
        _dbContext.Attach(data.Status);
        _dbContext.Orders.Add(data);
        _dbContext.SaveChanges();
    }


    public void Update(Order data)
    {
        _dbContext.Attach(data.Status);
        _dbContext.Entry(data).State = EntityState.Modified;

        _dbContext.SaveChanges();
    }

    public async Task<Order> GetByIdAsync(Guid orderId)
    {
        var data = await _dbContext
            .Orders
                .Include(x => x.Status)
            .Where(o => o.Id == orderId)
            .FirstOrDefaultAsync();

        return data;

    }

    public async Task<Order> GetAssignedOrderByCourierIdAsync(Guid courierId)
    {
        var data = await _dbContext
            .Orders
                .Include(x => x.Status)
            .Where(o => o.Status == Status.Assigned)
            .Where(o => o.CourierId == courierId)
            .FirstOrDefaultAsync();

        return data;
    
    }


    public IEnumerable<Order> GetAllNew()
    {
        var data = _dbContext
            .Orders
                .Include(x => x.Status)
            .Where(o => o.Status == Status.New);
        return data;
    }


}

