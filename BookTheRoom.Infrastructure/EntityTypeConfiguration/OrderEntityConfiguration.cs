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
    internal class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();
            builder.Property(o => o.OverallPrice).IsRequired();
            builder.Property(o => o.CheckIn).IsRequired();
            builder.Property(o => o.CheckOut).IsRequired();
            builder.HasOne(o => o.Hotel)
                   .WithMany()
                   .HasForeignKey(o => o.HotelId)
                   .IsRequired();
            builder.HasOne(o => o.Room)
                   .WithMany()
                   .HasForeignKey(o => o.RoomId)
                   .IsRequired();
        }
    }
}
