using Core.Entities;

namespace Core.Interfaces
{
    public interface IChatRepository
    {
        Task<List<Chat>> GetChatsByApartmentId(int apartmentId);
        Task<Chat> CreateChat(int apartmentId, List<string> userIds);
        Task<List<ChatMessage>> GetMessagesByChatId(Guid chatId);
        Task<ChatMessage> AddMessage(ChatMessage message);
    }
}
