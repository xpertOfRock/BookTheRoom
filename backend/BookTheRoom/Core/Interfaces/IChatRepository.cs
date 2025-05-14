using Core.Entities;

namespace Core.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> CreateChatAsync(string userId, CancellationToken token = default);
        Task<Chat?> GetChatAsync(Guid chatId, CancellationToken token = default);
        Task<IEnumerable<Chat>> GetChatsForAdminAsync(CancellationToken token = default);
        Task<ChatMessage> AddMessageAsync(Guid chatId, string userId, string userName, string text, CancellationToken token = default);
        Task<IEnumerable<ChatMessage>> GetMessagesAsync(Guid chatId, CancellationToken token = default);
    }
}
