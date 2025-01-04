using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder
                .Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder
                .Property(c => c.UserId)
                .IsRequired()           
                .HasMaxLength(50);

            builder
                .Property(c => c.UserScore)
                .IsRequired();

            builder
                .Property(c => c.HotelId)
                .IsRequired(false);

            builder
                .Property(c => c.Username)
                .IsRequired();

            builder
                .Property(c => c.ApartmentId)
                .IsRequired(false);               

            builder
                .Property(c => c.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(c => c.UpdatedAt)
                    .IsRequired(false);

            builder.HasIndex(c => c.UserId);
        }
    }
}