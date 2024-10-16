using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations
{
    class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Services");
            builder.HasKey(s => new { s.HotelId, s.OptionName });

            builder.Property(s => s.HotelId)
                .IsRequired();

            builder.Property(s => s.OptionName)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderOptionsName)Enum.Parse(typeof(OrderOptionsName), v)
                );

            builder.Property(s => s.OptionPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne<Hotel>()
                .WithMany(h => h.Services)
                .HasForeignKey(s => s.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
