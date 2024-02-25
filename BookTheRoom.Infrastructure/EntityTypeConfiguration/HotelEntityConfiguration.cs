using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTheRoom.Infrastructure.EntityTypeConfiguration
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
            builder.HasOne(h => h.Address)
                   .WithMany()
                   .HasForeignKey(h => h.AddressId)
                   .IsRequired();
        }
    }
}
