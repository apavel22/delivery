using DeliveryApp.Core.Domain.CourierAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.EntityConfigurations.CourierAggregate;

class StatusEntityTypeConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.ToTable("courier_statuses");

        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id)
        	.HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
             .UsePropertyAccessMode(PropertyAccessMode.Field)
             .HasColumnName("name")
             .IsRequired();
    }
}

