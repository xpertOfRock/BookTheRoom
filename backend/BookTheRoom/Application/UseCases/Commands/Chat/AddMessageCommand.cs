using Core.Entities;

namespace Application.UseCases.Commands.Chat
{
    public class AddMessageCommand : ICommand<ChatMessage>
    {
        public Guid ChatId { get; }
        public string ConnectionId { get; }
        public string UserId { get; }
        public string UserName { get; }
        public string Message { get; }
        public AddMessageCommand(Guid chatId, string connectionid, string userId, string username, string message)
        {
            ChatId = chatId;
            ConnectionId = connectionid;
            UserId = userId;
            UserName = username;
            Message = message;
        }
    }
}
