using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Mappers
{
    public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> builder)
        {
            builder.ToTable("Destination");
            builder.HasKey(l => l.DestinationId);
            builder.Property(l => l.IATA).IsRequired();
            builder.Property(l => l.City).IsRequired();
            builder.Property(l => l.Country).IsRequired();
        }
    }
}
