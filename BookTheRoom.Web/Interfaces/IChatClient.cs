namespace BookTheRoom.Web.Interfaces
{
    public interface IChatClient
    {
        public Task RecieveMessage(string username, string message);
    }
}
