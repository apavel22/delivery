using DeliveryApp.Core.Domain.CourierAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.EntityConfigurations.CourierAggregate;

public class CourierEntityTypeConfiguration : IEntityTypeConfiguration<Courier>
{
    public void Configure(EntityTypeBuilder<Courier> builder)
    {
        builder.ToTable("courier");

        builder.Ignore(entity => entity.DomainEvents);

        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(entity => entity.Name)
             .UsePropertyAccessMode(PropertyAccessMode.Field)
             .HasColumnName("name")
             .IsRequired();


        builder.HasOne(entity => entity.Transport)
            .WithMany()
            .IsRequired()
            .HasForeignKey("transport_id");

        builder.HasOne(entity => entity.Status)
            .WithMany()
            .IsRequired()
            .HasForeignKey("status_id");

        //		builder.OwnsOne(entity => entity.Location, 
        builder.ComplexProperty(entity => entity.Location,
                    a =>
                    {
                        a.Property(b => b.X).HasColumnName("location_x").IsRequired(true);
                        a.Property(b => b.Y).HasColumnName("location_y").IsRequired(true);
                        //		                a.WithOwner();
                    });

    }
}

