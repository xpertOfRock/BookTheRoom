namespace Api.Chat.Interfaces
{
    public interface IChatClient
    {
        Task ReceiveMessage(Guid id, string userId, string username, string message, DateTime createdAt, string connectionId);
    }
}
