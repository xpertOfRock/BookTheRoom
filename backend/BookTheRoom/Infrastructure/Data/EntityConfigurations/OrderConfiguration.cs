using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OverallPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.CheckIn)
                .IsRequired();

            builder.Property(o => o.CheckOut)
                .IsRequired();

            builder.Property(o => o.Email)
                .HasMaxLength(100);

            builder.Property(o => o.Phone)
                .HasMaxLength(20);

            builder.Property(o => o.UserId)
                .IsRequired();

            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(o => o.Room)
                .WithMany()
                .HasForeignKey(o => new { o.HotelId, o.RoomNumber })
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(o => o.UserId);
        }
    }
}
