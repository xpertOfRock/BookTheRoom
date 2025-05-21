using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EntityConfigurations
{
    class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.ToTable("Apartments");
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Id).ValueGeneratedOnAdd();

            builder.Property(h => h.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.Description)
                .IsRequired()
                .HasMaxLength(2000);

 
            builder.Property(h => h.OwnerId)               
                .IsRequired();

            builder.Property(h => h.Images)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            builder.Property(c => c.PriceForNight)
              .HasColumnType("decimal(18,2)")
              .IsRequired();

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

            builder.HasMany(h => h.Comments)
                .WithOne()
                .HasForeignKey(c => c.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Chats)
                .WithOne()
                .HasForeignKey(c => c.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
