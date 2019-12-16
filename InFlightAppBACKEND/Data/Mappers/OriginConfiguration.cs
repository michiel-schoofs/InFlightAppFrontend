using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Mappers
{
    public class OriginConfiguration : IEntityTypeConfiguration<Origin>
    {
        public void Configure(EntityTypeBuilder<Origin> builder)
        {
            builder.ToTable("Origin");
            builder.HasKey(l => l.OriginId);
            builder.Property(l => l.IATA).IsRequired();
            builder.Property(l => l.City).IsRequired();
            builder.Property(l => l.Country).IsRequired();
        }
    }
}
