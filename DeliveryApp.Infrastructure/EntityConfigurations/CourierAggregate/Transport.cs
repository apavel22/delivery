using DeliveryApp.Core.Domain.CourierAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.EntityConfigurations.CourierAggregate;

class TransportEntityTypeConfiguration : IEntityTypeConfiguration<Transport>
{
    public void Configure(EntityTypeBuilder<Transport> builder)
    {
        builder.ToTable("transport");

        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id)
        	.HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
             .UsePropertyAccessMode(PropertyAccessMode.Field)
             .HasColumnName("name")
             .IsRequired();

		builder.OwnsOne(entity => entity.Speed, 
					a =>
            		{
                		a.Property(b => b.Value).HasColumnName("speed").IsRequired();
		                a.WithOwner();
        		    });

		builder.OwnsOne(entity => entity.Capacity, 
					a =>
            		{
                		a.Property(b => b.Value).HasColumnName("capacity").IsRequired();
		                a.WithOwner();
        		    });
    }
}

