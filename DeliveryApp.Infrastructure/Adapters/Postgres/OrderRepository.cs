using System;
using Microsoft.EntityFrameworkCore;

using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Ports;

using Primitives;

namespace DeliveryApp.Infrastructure.Adapters.Postgres;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Order Add(Order data)
    {
//        _dbContext.Attach(basket.Status);
        return _dbContext.Orders.Add(data).Entity;
    }


    public void Update(Order data)
    {
//        _dbContext.Attach(basket.Status);
        _dbContext.Entry(data).State = EntityState.Modified;
    }

    public async Task<Order> GetByIdAsync(Guid Id)
    {
        var data = await _dbContext
            .Orders
//            .Include(x => x.Address)
//            .Include(x => x.TimeSlot)
//            .Include(x => x.Status)
            .FirstOrDefaultAsync(o => o.Id == Id);

        return data;

	}

    public async Task<IEnumerable<Order>> GetAllUnassignedAsync()
    {
        var data = await _dbContext
            .Orders
            	.Where(o => o.Status == Status.Created)
            .ToListAsync();

        return data;
    }

}

