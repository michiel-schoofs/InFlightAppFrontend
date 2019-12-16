using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Mappers
{
    public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.ToTable("OrderLine");
            builder.HasKey(ol => new { ol.OrderId, ol.ProductId });
            builder.Property(ol => ol.Amount).IsRequired();
            builder.HasOne(ol => ol.Product)
                .WithMany()
                .IsRequired()
                .HasForeignKey(ol => ol.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ol => ol.Order)
                .WithMany(o => o.OrderLines)
                .IsRequired()
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
