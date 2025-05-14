
namespace Infrastructure.Data.Repositories
{
    public class ChatRepository(ApplicationDbContext context) : IChatRepository
    {
        public async Task<Chat> CreateChatAsync(string userId, CancellationToken ct = default)
        {
            var chat = new Chat();
            chat.UsersId.Add(userId);
            context.Chats.Add(chat);
            await context.SaveChangesAsync(ct);
            return chat;
        }

        public async Task<Chat?> GetChatAsync(Guid chatId, CancellationToken ct = default) =>
            await context.Chats
                     .Include(c => c.Messages)
                     .FirstOrDefaultAsync(c => c.Id == chatId, ct);

        public async Task<IEnumerable<Chat>> GetChatsForAdminAsync(CancellationToken ct = default) =>
            await context.Chats
                     .Include(c => c.Messages)
                     .ToListAsync(ct);

        public async Task<ChatMessage> AddMessageAsync(Guid chatId, string userId, string userName, string text, CancellationToken ct = default)
        {
            var msg = new ChatMessage
            {
                ChatId = chatId,
                UserId = userId,
                UserName = userName,
                Message = text
            };
            context.ChatMessages.Add(msg);
            await context.SaveChangesAsync(ct);
            return msg;
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesAsync(Guid chatId, CancellationToken ct = default) =>
            await context.ChatMessages
                     .Where(m => m.ChatId == chatId)
                     .OrderBy(m => m.CreatedAt)
                     .ToListAsync(ct);
    }
}
}
