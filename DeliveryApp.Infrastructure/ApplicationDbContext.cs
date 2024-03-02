using System.Data;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.CourierAggregate;


namespace DeliveryApp.Infrastructure;

public class ApplicationDbContext : DbContext //, IUnitOfWork
{
	public DbSet<Order> Orders { get; set; }
	public DbSet<Courier> Couriers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}
