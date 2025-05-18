
namespace Infrastructure.Data.Repositories
{
    public class ChatRepository(ApplicationDbContext context) : IChatRepository
    {
        public async Task<List<Chat>> GetChatsByApartmentId(int apartmentId)
        {
            return await context.Chats
                .Where(c => c.ApartmentId == apartmentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Chat> CreateChat(int apartmentId, List<string> userIds)
        {
            var chat = new Chat
            {
                ApartmentId = apartmentId,
                UsersId = userIds
            };
            await context.Chats.AddAsync(chat);
            await context.SaveChangesAsync();
            return chat;
        }

        public async Task<List<ChatMessage>> GetMessagesByChatId(Guid chatId)
        {
            return await context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ChatMessage> AddMessage(ChatMessage message)
        {
            await context.Messages.AddAsync(message);
            await context.SaveChangesAsync();
            return message;
        }
    }
}

