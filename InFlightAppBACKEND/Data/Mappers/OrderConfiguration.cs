using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Mappers
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(o => o.OrderId);

            builder
                .HasOne(o => o.Passenger)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PassengerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(o => o.PassengerId).IsRequired();
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.IsDone).IsRequired();
        }
    }
}
