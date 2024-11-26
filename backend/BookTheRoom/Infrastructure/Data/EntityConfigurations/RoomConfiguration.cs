using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => new { r.HotelId, r.Number });

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(r => r.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.Category)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (RoomCategory)Enum.Parse(typeof(RoomCategory), v)
                );

            builder.Property(r => r.Images)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                );
        }
    }
}
