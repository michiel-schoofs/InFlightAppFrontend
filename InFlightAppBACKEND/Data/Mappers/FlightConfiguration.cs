using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Mappers
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.ToTable("Flight");
            builder.HasKey(f => f.FlightId);
            builder.Property(f => f.FlightNr).IsRequired();
            builder.Property(f => f.Plane).IsRequired();
            builder.Property(f => f.DepartureTime).IsRequired();
            builder.Property(f => f.ArrivalTime).IsRequired();

            builder.HasOne(f => f.Origin).WithOne().IsRequired().HasForeignKey<Origin>(o => o.FlightId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(f => f.Destination).WithOne().IsRequired().HasForeignKey<Destination>(d => d.FlightId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(f => f.Seats)
                .WithOne()
                .IsRequired()
                .HasForeignKey(s => s.FlightId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
