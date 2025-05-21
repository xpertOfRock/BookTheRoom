using Core.Entities;

namespace Core.Interfaces
{
    public interface IChatRepository
    {
        Task<List<Chat>> GetChatsByApartmentId(int apartmentId);
        Task<Chat> CreateChat(List<string> userIds, int? apartmentId);
        Task<List<ChatMessage>> GetMessagesByChatId(string chatId);
        Task<ChatMessage> AddMessage(ChatMessage message);
    }
}
