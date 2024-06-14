using BookTheRoom.Web.Interfaces;
using BookTheRoom.Web.Models;
using Microsoft.AspNetCore.SignalR;

namespace BookTheRoom.Web.Chat
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task JoinChat(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

            await Clients
                .Group(connection.ChatRoom)
                .RecieveMessage(connection.Username, $"User {connection.Username} entered the chat.");
        }
    }
    
}
