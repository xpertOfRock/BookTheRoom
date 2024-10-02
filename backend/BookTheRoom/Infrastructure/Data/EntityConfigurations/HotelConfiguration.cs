using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.ToTable("Hotels");
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Id).ValueGeneratedOnAdd();

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(h => h.Rating)
                .IsRequired();

            builder.Property(h => h.HasPool)
                .IsRequired();

            builder.Property(h => h.Images)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            builder.OwnsOne(h => h.Address, a =>
            {
                a.Property(ad => ad.Street)
                    .IsRequired()
                    .HasMaxLength(200);

                a.Property(ad => ad.City)
                    .IsRequired()
                    .HasMaxLength(100);

                a.Property(ad => ad.State)
                    .HasMaxLength(100);

                a.Property(ad => ad.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                a.Property(ad => ad.PostalCode)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            builder.HasMany(h => h.Rooms)
                .WithOne()
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Comments)
                .WithOne()               
                .HasForeignKey(c => c.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
