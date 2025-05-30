using Core.Entities;

namespace Core.Interfaces
{
    public interface IChatRepository
    {
        Task<List<Chat>> GetChatsByApartmentId(int apartmentId, CancellationToken token = default);
        Task<Chat> CreateChat(List<string> userIds, int? apartmentId, CancellationToken token = default);
        Task<Chat?> GetExistingChatByUserId(string userId, int apartmentId, CancellationToken token = default);
        Task AddMessage(ChatMessage message, CancellationToken token = default);
        Task UpdateCache(Chat chat, CancellationToken token = default);
        Task<Chat> GetChatById(Guid chatId, CancellationToken token = default);
    }
}
