using BookTheRoom.Domain.Entities;
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
            builder.Property(h => h.Name).IsRequired();
            builder.Property(h => h.Description).IsRequired();
            builder.Property(h => h.Rating).IsRequired();
            builder.Property(h => h.HasPool).IsRequired();
            builder.Property(h => h.Image).IsRequired();
            builder.HasOne(h => h.Address)
                   .WithOne()
                   .HasForeignKey<Hotel>(h => h.AddressId)
                   .IsRequired();
        }
    }
}
