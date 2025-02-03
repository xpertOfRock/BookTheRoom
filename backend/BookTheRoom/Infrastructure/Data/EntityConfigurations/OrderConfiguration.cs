using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                .IsRequired(false);

            builder.Property(o => o.FirstName)
                .IsRequired();

            builder.Property(o => o.LastName)
                .IsRequired();

            builder.Property(o => o.HotelId)
                .IsRequired();

            builder.Property(o => o.RoomNumber)
                .IsRequired();

            builder.HasIndex(o => o.UserId);
        }
    }
}
