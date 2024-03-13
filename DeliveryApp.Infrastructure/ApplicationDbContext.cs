using System.Data;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.CourierAggregate;

using CourierStatus = DeliveryApp.Core.Domain.CourierAggregate;
using OrderStatus = DeliveryApp.Core.Domain.OrderAggregate;

using DeliveryApp.Infrastructure.EntityConfigurations.CourierAggregate;
using DeliveryApp.Infrastructure.EntityConfigurations.OrderAggregate;

using CourierStatusConfiguration = DeliveryApp.Infrastructure.EntityConfigurations.CourierAggregate;
using OrderStatusConfiguration = DeliveryApp.Infrastructure.EntityConfigurations.OrderAggregate;

using Primitives;

namespace DeliveryApp.Infrastructure;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Courier> Couriers { get; set; }

    //	public DbSet<CourierStatus.Status> CourierStatuses { get; set; }

    private IDbContextTransaction _currentTransaction;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Courier aggregate
        modelBuilder.ApplyConfiguration(new CourierEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CourierStatusConfiguration.StatusEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TransportEntityTypeConfiguration());

        // Courier statuses
        modelBuilder.Entity<CourierStatus.Status>(b =>
        {
            var allStatuses = CourierStatus.Status.List();
            b.HasData(allStatuses.Select(c => new { c.Id, c.Name }));
        });

        //Courier transports
        modelBuilder.Entity<Transport>(b =>
        {
            var allTransports = Transport.List();
            b.HasData(allTransports.Select(c => new { Id = c.Id, Name = c.Name }));
            b.OwnsOne(e => e.Capacity).HasData(allTransports.Select(c => new { TransportId = c.Id, c.Capacity.Value }));
            b.OwnsOne(e => e.Speed).HasData(allTransports.Select(c => new { TransportId = c.Id, c.Speed.Value }));
        });


        // Order aggregate
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderStatusConfiguration.StatusEntityTypeConfiguration());

        //Status
        modelBuilder.Entity<OrderStatus.Status>(b =>
        {
            var allStatuses = OrderStatus.Status.List();
            b.HasData(allStatuses.Select(c => new { c.Id, c.Name }));
        });


        //		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}



