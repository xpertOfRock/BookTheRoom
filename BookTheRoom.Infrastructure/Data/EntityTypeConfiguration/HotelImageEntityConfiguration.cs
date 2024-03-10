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
            builder.Property(ri => ri.URL).HasMaxLength(1000).IsRequired();
        }
    }    
}
