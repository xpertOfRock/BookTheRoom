using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTheRoom.Infrastructure.Data.EntityTypeConfiguration
{
    internal class RoomEntityConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();
            builder.Property(r => r.Number).IsRequired();
            builder.Property(h => h.Description).HasMaxLength(1000).IsRequired();
            builder.Property(r => r.PriceForRoom).HasColumnType("decimal(10,5)").IsRequired();
            builder.Property(r => r.IsFree).IsRequired();
            builder.Property(r => r.HotelId).IsRequired();
            builder.Property(r => r.PreviewURL).HasMaxLength(500).IsRequired();

            builder.HasOne(r => r.Hotel)
                   .WithMany(h => h.Rooms)
                   .HasForeignKey(r => r.HotelId)
                   .IsRequired();
        }
    }
}
