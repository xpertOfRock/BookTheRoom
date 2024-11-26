using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Services");
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.ServiceName)
                .IsRequired();

            builder
                .Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}
