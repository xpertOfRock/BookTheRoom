
namespace Infrastructure.Data.Repositories
{
    public class ChatRepository(
        ApplicationDbContext context,
        IDistributedCache distributedCache) : IChatRepository
    {
        public async Task<List<Chat>> GetChatsByApartmentId(int apartmentId)
        {
            return await context.Chats
                .Where(c => c.ApartmentId == apartmentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Chat> CreateChat(List<string> userIds, int? apartmentId = null)
        {
            var chat = new Chat
            {
                ApartmentId = apartmentId,
                UsersId = userIds
            };
            var result = await context.Chats.AddAsync(chat);

            return result.Entity;
        }

        public async Task<List<ChatMessage>> GetMessagesByChatId(string chatId)
        {
            Guid.TryParse(chatId, out Guid result);

            return await context.Messages
                .Where(m => m.ChatId == result)
                .OrderBy(m => m.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ChatMessage> AddMessage(ChatMessage message)
        {
            var result = await context.Messages.AddAsync(message);

            return result.Entity;
        }
    }
}

