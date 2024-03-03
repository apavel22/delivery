using DeliveryApp.Core.Domain.OrderAggregate;

using DeliveryApp.Core.Domain.CourierAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.EntityConfigurations.OrderAggregate;


public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("order");

        builder.Ignore(entity => entity.DomainEvents);

        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id)
        	.HasColumnName("id")
            .ValueGeneratedNever();


		builder.OwnsOne(entity => entity.Location, 
					a =>
            		{
                		a.Property(b => b.X).HasColumnName("location_x").IsRequired(true);
                		a.Property(b => b.Y).HasColumnName("location_y").IsRequired(true);
		                a.WithOwner();
        		    });

		builder.OwnsOne(entity => entity.Weight, 
					a =>
            		{
                		a.Property(b => b.Value).HasColumnName("weight").IsRequired(true);
		                a.WithOwner();
        		    });

		builder.Property(entity => entity.CourierId)
	            .HasColumnName("courier_id")
	            .IsRequired(false);

		builder.HasOne<Courier>()
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(entity => entity.CourierId)
            .HasPrincipalKey(courier => courier.Id);

            
		builder.HasOne(entity => entity.Status)
            .WithMany()
            .IsRequired()
            .HasForeignKey("status_id");

    }
}

