using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Mappers
{
    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.ToTable("Passenger");
            builder.HasKey(p => p.UserId);
            
            builder.Ignore(p => p.Username);
            builder.Ignore(p => p.Password);

            builder.Property(p => p.FirstName).IsRequired();
            builder.Property(p => p.LastName).IsRequired();

            builder.HasOne(u => u.ProfilePicture)
                .WithOne()
                .HasForeignKey<Passenger>(u => u.ProfilePictureID)
                .HasPrincipalKey<Image>(i => i.ID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
