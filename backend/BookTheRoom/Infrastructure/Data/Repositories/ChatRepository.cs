namespace Infrastructure.Data.Repositories
{
    public class ChatRepository(
        ApplicationDbContext context,
        IDistributedCache distributedCache) : IChatRepository
    {
        public async Task<List<Chat>> GetChatsByApartmentId(int apartmentId, CancellationToken token = default)
        {
            return await context.Chats
                .Where(c => c.ApartmentId == apartmentId)
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<Chat> CreateChat(List<string> userIds, int? apartmentId = null, CancellationToken token = default)
        {
            var chat = new Chat
            {
                Id = Guid.NewGuid(),
                ApartmentId = apartmentId,
                UsersId = userIds
            };
            var result = await context.Chats.AddAsync(chat, token);

            return result.Entity;
        }
        public async Task<Chat> GetChatById(Guid chatId, CancellationToken token = default)
        {
            string key = $"chat-{chatId}";
            string? cachedHotel = await distributedCache.GetStringAsync(key, token);

            Chat? chat;

            if (string.IsNullOrEmpty(cachedHotel))
            {
                chat = await context.Chats
                    .AsNoTracking()
                    .Include(x => x.Messages)
                    .FirstOrDefaultAsync(x => x.Id == chatId, token);

                if (chat is null) throw new EntityNotFoundException<Chat>();

                await distributedCache.SetStringAsync
                (
                    key,
                    JsonConvert.SerializeObject(chat),
                    token
                );

                return chat;
            }

            chat = JsonConvert.DeserializeObject<Chat>(cachedHotel);

            if (chat is null) throw new EntityNotFoundException<Chat>();

            return chat;
        }

        public async Task AddMessage(ChatMessage message, CancellationToken token = default)
        {
            await context.Messages.AddAsync(message);
        }

        public async Task<Chat?> GetExistingChatByUserId(string userId, int apartmentId, CancellationToken token = default)
        {
            return await context.Chats
                .AsNoTracking()
                .Include(x => x.Messages)
                .FirstOrDefaultAsync(x => 
                    x.UsersId.Contains(userId) &&
                    x.ApartmentId != null &&
                    x.ApartmentId == apartmentId, token);
        }

        public async Task UpdateCache(Chat chat, CancellationToken token = default)
        {
            string key = $"chat-{chat.Id}";

            string? cachedChat = await distributedCache.GetStringAsync(key, token);

            if (string.IsNullOrEmpty(cachedChat)) return;

            await distributedCache.SetStringAsync
            (
                key,
                JsonConvert.SerializeObject(chat),
                token
            );
        }
    }
}

