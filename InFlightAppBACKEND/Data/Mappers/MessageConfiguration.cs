using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Mappers
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");
            builder.HasKey(m => m.MessageId);
            builder.Property(m => m.ConversationId).IsRequired();
            builder.Property(m => m.Content).IsRequired();
            builder.Property(m => m.DateSent).IsRequired();

            #region Associations
                builder
                    .HasOne(m => m.Sender)
                    .WithMany()
                    .IsRequired()
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.Restrict); 
            #endregion
        }
    }
}
