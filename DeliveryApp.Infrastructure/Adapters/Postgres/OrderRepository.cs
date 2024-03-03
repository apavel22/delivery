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
        _dbContext.Attach(data.Status);
        var dataOut = _dbContext.Orders.Add(data).Entity;

        _dbContext.SaveChanges();

        return dataOut;
    }


    public void Update(Order data)
    {
        _dbContext.Attach(data.Status);
        _dbContext.Entry(data).State = EntityState.Modified;

        _dbContext.SaveChanges();
    }

    public async Task<Order> GetByIdAsync(Guid Id)
    {
        var data = await _dbContext
            .Orders
	            .Include(x => x.Status)
            .FirstOrDefaultAsync(o => o.Id == Id);

/*
            if (data == null)
            {
                data = _dbContext
                    .Orders
                    .Local
    	        .FirstOrDefault(o => o.Id == Id);
            }
*/
        return data;

	}

    public IEnumerable<Order> GetAllUnassigned()
    {
        var data = _dbContext.Orders.Where(o => o.Status == Status.Created);

        return data;
    }

}

