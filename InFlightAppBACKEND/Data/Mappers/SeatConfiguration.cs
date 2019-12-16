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
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.ToTable("Seat");
            builder.HasKey(s => s.SeatId);
            builder.Property(s => s.FlightId).IsRequired();
            builder.Property(s => s.Type).IsRequired();
            builder.HasOne(s => s.Passenger)
                .WithOne(p => p.Seat)
                .IsRequired(false)
                .HasForeignKey<Seat>(s=>s.PassengerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
