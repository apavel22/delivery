﻿// <auto-generated />
using System;
using DeliveryApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeliveryApp.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Courier", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("status_id")
                        .HasColumnType("integer");

                    b.Property<int>("transport_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("status_id");

                    b.HasIndex("transport_id");

                    b.ToTable("courier", (string)null);
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Status", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("courier_statuses", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "notavailable"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ready"
                        },
                        new
                        {
                            Id = 3,
                            Name = "busy"
                        });
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Transport", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("transport", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "pedestrian"
                        },
                        new
                        {
                            Id = 2,
                            Name = "bicycle"
                        },
                        new
                        {
                            Id = 3,
                            Name = "scooter"
                        },
                        new
                        {
                            Id = 4,
                            Name = "car"
                        });
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.OrderAggregate.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("CourierId")
                        .HasColumnType("uuid")
                        .HasColumnName("courier_id");

                    b.Property<int>("status_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CourierId");

                    b.HasIndex("status_id");

                    b.ToTable("order", (string)null);
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.OrderAggregate.Status", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("order_statuses", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "created"
                        },
                        new
                        {
                            Id = 2,
                            Name = "assigned"
                        },
                        new
                        {
                            Id = 3,
                            Name = "completed"
                        });
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Courier", b =>
                {
                    b.HasOne("DeliveryApp.Core.Domain.CourierAggregate.Status", "Status")
                        .WithMany()
                        .HasForeignKey("status_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryApp.Core.Domain.CourierAggregate.Transport", "Transport")
                        .WithMany()
                        .HasForeignKey("transport_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("DeliveryApp.Core.Domain.SharedKernel.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("CourierId")
                                .HasColumnType("uuid");

                            b1.Property<int>("X")
                                .HasColumnType("integer")
                                .HasColumnName("location_x");

                            b1.Property<int>("Y")
                                .HasColumnType("integer")
                                .HasColumnName("location_y");

                            b1.HasKey("CourierId");

                            b1.ToTable("courier");

                            b1.WithOwner()
                                .HasForeignKey("CourierId");
                        });

                    b.Navigation("Location")
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("Transport");
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.CourierAggregate.Transport", b =>
                {
                    b.OwnsOne("DeliveryApp.Core.Domain.SharedKernel.Weight", "Capacity", b1 =>
                        {
                            b1.Property<int>("TransportId")
                                .HasColumnType("integer");

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("capacity");

                            b1.HasKey("TransportId");

                            b1.ToTable("transport");

                            b1.WithOwner()
                                .HasForeignKey("TransportId");

                            b1.HasData(
                                new
                                {
                                    TransportId = 1,
                                    Value = 1
                                },
                                new
                                {
                                    TransportId = 2,
                                    Value = 4
                                },
                                new
                                {
                                    TransportId = 3,
                                    Value = 6
                                },
                                new
                                {
                                    TransportId = 4,
                                    Value = 8
                                });
                        });

                    b.OwnsOne("DeliveryApp.Core.Domain.SharedKernel.Speed", "Speed", b1 =>
                        {
                            b1.Property<int>("TransportId")
                                .HasColumnType("integer");

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("speed");

                            b1.HasKey("TransportId");

                            b1.ToTable("transport");

                            b1.WithOwner()
                                .HasForeignKey("TransportId");

                            b1.HasData(
                                new
                                {
                                    TransportId = 1,
                                    Value = 1
                                },
                                new
                                {
                                    TransportId = 2,
                                    Value = 2
                                },
                                new
                                {
                                    TransportId = 3,
                                    Value = 3
                                },
                                new
                                {
                                    TransportId = 4,
                                    Value = 4
                                });
                        });

                    b.Navigation("Capacity")
                        .IsRequired();

                    b.Navigation("Speed")
                        .IsRequired();
                });

            modelBuilder.Entity("DeliveryApp.Core.Domain.OrderAggregate.Order", b =>
                {
                    b.HasOne("DeliveryApp.Core.Domain.CourierAggregate.Courier", null)
                        .WithMany()
                        .HasForeignKey("CourierId");

                    b.HasOne("DeliveryApp.Core.Domain.OrderAggregate.Status", "Status")
                        .WithMany()
                        .HasForeignKey("status_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("DeliveryApp.Core.Domain.SharedKernel.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<int>("X")
                                .HasColumnType("integer")
                                .HasColumnName("location_x");

                            b1.Property<int>("Y")
                                .HasColumnType("integer")
                                .HasColumnName("location_y");

                            b1.HasKey("OrderId");

                            b1.ToTable("order");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsOne("DeliveryApp.Core.Domain.SharedKernel.Weight", "Weight", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("weight");

                            b1.HasKey("OrderId");

                            b1.ToTable("order");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("Location")
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("Weight")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}