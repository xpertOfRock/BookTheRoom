namespace Api.Chat.Interfaces
{
    public interface IChatClient
    {
        public Task RecieveMessage(string userName, string message);

    }
}
