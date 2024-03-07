using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTheRoom.Infrastructure.Data.EntityTypeConfiguration
{
    public class HotelImageEntityConfiguration : IEntityTypeConfiguration<HotelImage>
    {
        public void Configure(EntityTypeBuilder<HotelImage> builder)
        {
            builder.HasKey(hi => hi.Id);
            builder.Property(hi => hi.Id).ValueGeneratedOnAdd();
            builder.Property(hi => hi.URL).IsRequired().HasMaxLength(255);
            builder.Property(hi => hi.HotelId).IsRequired();

            builder.HasOne(hi => hi.Hotel)
                   .WithMany(h => h.HotelImages)
                   .HasForeignKey(hi => hi.HotelId)
                   .IsRequired();
        }
    }    
}
