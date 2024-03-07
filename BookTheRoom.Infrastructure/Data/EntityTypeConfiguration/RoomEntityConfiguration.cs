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
            builder.Property(r => r.PriceForRoom).HasColumnType("decimal(10,5)").IsRequired();
            builder.Property(r => r.IsFree).IsRequired();
            builder.Property(r => r.HotelId).IsRequired();

            builder.HasOne(r => r.PreviewImage)
                   .WithOne()
                   .HasForeignKey<Room>(r => r.PreviewImageId)
                   .IsRequired();

            builder.HasOne(r => r.Hotel)
                   .WithMany(h => h.Rooms)
                   .HasForeignKey(r => r.HotelId)
                   .IsRequired();


            builder.HasMany(r => r.RoomImages)
                   .WithOne(ri => ri.Room)
                   .HasForeignKey(ri => ri.RoomId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
