using BookTheRoom.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTheRoom.Infrastructure.Data.EntityTypeConfiguration
{
    public class HotelEntityConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Id).ValueGeneratedOnAdd();
            builder.Property(h => h.Name).IsRequired().HasMaxLength(100);
            builder.Property(h => h.Description).HasMaxLength(1000).IsRequired();
            builder.Property(h => h.Rating).IsRequired();
            builder.Property(h => h.HasPool).IsRequired();
            builder.Property(h => h.NumberOfRooms).IsRequired();
            builder.Property(h => h.PreviewURL).HasMaxLength(500).IsRequired();

            builder.HasMany(h => h.Rooms)
                   .WithOne(r => r.Hotel)
                   .HasForeignKey(r => r.HotelId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(h => h.Address, address =>
            {
                address.Property(a => a.Country).IsRequired().HasMaxLength(100);
                address.Property(a => a.City).IsRequired().HasMaxLength(100);
                address.Property(a => a.StreetOrDistrict).IsRequired().HasMaxLength(200);
                address.Property(a => a.Index).IsRequired();
            });
        }
    }
}
