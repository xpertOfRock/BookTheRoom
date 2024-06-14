using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTheRoom.Infrastructure.Data.EntityTypeConfiguration
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();
            builder.Property(o => o.OverallPrice).HasColumnType("decimal(10,5)").IsRequired();
            builder.Property(o => o.RoomId).IsRequired();
            builder.Property(o => o.HotelId).IsRequired();
            builder.Property(o => o.CheckIn).IsRequired();
            builder.Property(o => o.CheckOut).IsRequired();
            builder.Property(o => o.Status).IsRequired();           

        }
    }
}
