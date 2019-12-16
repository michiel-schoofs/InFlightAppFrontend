using Microsoft.EntityFrameworkCore;
using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InFlightAppBACKEND.Data.Mappers{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>{
        public void Configure(EntityTypeBuilder<Image> builder){
            builder.ToTable("Image");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.Data).IsRequired();
        }
    }
}
