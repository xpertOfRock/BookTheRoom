using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder
                .ToTable("Chats");

            builder
                .HasKey(x => x.Id);

            builder
                .HasMany(h => h.Messages)
                .WithOne()
                .HasForeignKey(c => c.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
