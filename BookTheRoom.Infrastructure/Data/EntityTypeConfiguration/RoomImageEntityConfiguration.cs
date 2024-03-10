

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
            builder.Property(ri => ri.Id).ValueGeneratedOnAdd();
            builder.Property(ri => ri.URL).HasMaxLength(500).IsRequired();
        }       
    }
}
