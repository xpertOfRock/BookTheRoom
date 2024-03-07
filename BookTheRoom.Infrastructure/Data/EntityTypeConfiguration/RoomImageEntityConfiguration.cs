

using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookTheRoom.Infrastructure.Data.EntityTypeConfiguration
{
    public class RoomImageEntityConfiguration: IEntityTypeConfiguration<RoomImage>
    {
        public void Configure(EntityTypeBuilder<RoomImage> builder)
        {
            builder.HasKey(ri => ri.Id);
            builder.Property(h => h.Id).ValueGeneratedOnAdd();
            builder.Property(ri => ri.URL).IsRequired().HasMaxLength(255);
            builder.Property(ri => ri.RoomId).IsRequired();

            builder.HasOne(ri => ri.Room)
                   .WithMany(r => r.RoomImages)
                   .HasForeignKey(ri => ri.RoomId)
                   .IsRequired();
        }
        
    }
}
