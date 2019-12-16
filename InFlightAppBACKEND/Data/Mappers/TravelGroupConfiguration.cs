using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Mappers
{
    public class TravelGroupConfiguration : IEntityTypeConfiguration<TravelGroup>
    {
        public void Configure(EntityTypeBuilder<TravelGroup> builder)
        {
            builder.ToTable("TravelGroup");
            builder.HasKey(tg => tg.TravelGroupId);
            builder.HasOne(tg => tg.Conversation)
                .WithOne()
                .IsRequired()
                .HasForeignKey<Conversation>(c=>c.TravelGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(tg => tg.Passengers)
                .WithOne(p => p.TravelGroup)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
