using Core.Entities;

namespace Core.Interfaces
{
    public interface IChatRepository
    {
        Task<List<Chat>> GetChatsByApartmentIdAsync(int apartmentId);
        Task<Chat> CreateChatAsync(int apartmentId, string userId, string ownerId);
        Task<List<ChatMessage>> GetMessagesByChatIdAsync(Guid chatId);
        Task<ChatMessage> AddMessageAsync(ChatMessage message);
    }
}
