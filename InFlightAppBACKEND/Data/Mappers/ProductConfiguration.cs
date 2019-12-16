using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Mappers
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.UnitPrice).IsRequired();
            builder.Property(p => p.Type).IsRequired();

            builder.HasOne(p => p.Image).WithOne()
                .HasForeignKey<Product>(p => p.ImageID).HasPrincipalKey<Image>(i => i.ID)
                .OnDelete(DeleteBehavior.Cascade).IsRequired(false);
        }
    }
}
