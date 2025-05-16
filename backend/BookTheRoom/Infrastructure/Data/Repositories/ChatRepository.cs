
namespace Infrastructure.Data.Repositories
{
    public class ChatRepository(ApplicationDbContext context) : IChatRepository
    {
        public async Task<List<Chat>> GetChatsByApartmentIdAsync(int apartmentId)
        {
            return await context.Chats
                .Where(c => c.ApartmentId == apartmentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Chat> CreateChatAsync(int apartmentId, string userId, string ownerId)
        {
            var chat = new Chat
            {
                ApartmentId = apartmentId,
                UsersId = new List<string> { userId, ownerId }
            };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();
            return chat;
        }

        public async Task<List<ChatMessage>> GetMessagesByChatIdAsync(Guid chatId)
        {
            return await context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ChatMessage> AddMessageAsync(ChatMessage message)
        {
            context.Messages.Add(message);
            await context.SaveChangesAsync();
            return message;
        }
    }
}

