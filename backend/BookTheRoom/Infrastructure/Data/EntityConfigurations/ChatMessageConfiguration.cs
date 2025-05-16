using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder
                .ToTable("ChatMessages");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.UserName)
                .HasMaxLength(100);

            builder
                .Property(x => x.Message)
                .HasMaxLength(2000);
        }
    }
}
