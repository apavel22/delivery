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
        _dbContext.Attach(data.Status);
        _dbContext.Attach(data.Transport);

        var dataOut = _dbContext.Couriers.Add(data).Entity;

        _dbContext.SaveChanges();

        return dataOut;
    }


    public void Update(Courier data)
    {
        _dbContext.Attach(data.Status);
        _dbContext.Attach(data.Transport);
        _dbContext.Entry(data).State = EntityState.Modified;

        _dbContext.SaveChanges();
    }


    /// <summary>
    /// hack. Обновить без сохранния в БД
    /// </summary>
    /// <param name="data"></param>
    public void Update_WothoutSaveToDb_Hack(Courier data)
    {
        _dbContext.Attach(data.Status);
        _dbContext.Attach(data.Transport);
        _dbContext.Entry(data).State = EntityState.Modified;
    }

    public async Task<Courier> GetByIdAsync(Guid Id)
    {
        var data = await _dbContext
            .Couriers
                .Include(x => x.Status)
	            .Include(x => x.Transport)
            .FirstOrDefaultAsync(o => o.Id == Id);
/*
		if (data == null)
		{
			data = _dbContext
                    .Couriers
                    .Local
            .FirstOrDefault(o => o.Id == Id);
		}
*/
        return data;
	}

    public IEnumerable<Courier> GetAllReady()
    {
        var data =  _dbContext.Couriers.Where(o => o.Status == Status.Ready);

		return data;
    }

}

