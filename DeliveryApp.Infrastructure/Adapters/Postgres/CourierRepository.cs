using System;
using Microsoft.EntityFrameworkCore;


using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Ports;


using Primitives;

namespace DeliveryApp.Infrastructure.Adapters.Postgres;

public class CourierRepository : ICourierRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CourierRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }


    public Courier Add(Courier data)
    {
//        _dbContext.Attach(basket.Status);
        return _dbContext.Couriers.Add(data).Entity;
    }


    public void Update(Courier data)
    {
//        _dbContext.Attach(basket.Status);
        _dbContext.Entry(data).State = EntityState.Modified;
    }

    public async Task<Courier> GetByIdAsync(Guid Id)
    {
        var data = await _dbContext
            .Couriers
//            .Include(x => x.Address)
//            .Include(x => x.TimeSlot)
//            .Include(x => x.Status)
            .FirstOrDefaultAsync(o => o.Id == Id);

        return data;
	}

    public async Task<IEnumerable<Courier>> GetAllReadyAsync()
    {
        var data = await _dbContext
            .Couriers
            	.Where(o => o.Status == Status.Ready)
            .ToListAsync();
		return data;
    }

}

