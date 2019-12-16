using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Mappers
{
    public class CrewMemberConfiguration : IEntityTypeConfiguration<CrewMember>
    {
        public void Configure(EntityTypeBuilder<CrewMember> builder)
        {
            builder.ToTable("CrewMember");
            builder.HasKey(c => c.UserId);
            builder.Ignore(c => c.Username);
            builder.Property(c => c.FirstName).IsRequired();
            builder.Property(c => c.LastName).IsRequired();
            builder.HasOne(u => u.ProfilePicture)
                    .WithOne()
                    .HasForeignKey<CrewMember>(u => u.ProfilePictureID)
                    .HasPrincipalKey<Image>(i => i.ID)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
