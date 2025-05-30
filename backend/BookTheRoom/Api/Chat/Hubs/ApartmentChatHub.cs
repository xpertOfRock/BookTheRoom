using Api.Chat.Interfaces;
using Api.Chat.Models;
using Application.UseCases.Commands.Chat;
using Microsoft.AspNetCore.SignalR;

namespace Api.Chat.Hubs
{
    [Authorize]
    public class ApartmentChatHub(ISender sender) : Hub<IChatClient>
    {
        public async Task JoinChat(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatId);

            await Clients
                .Group(connection.ChatId)
                .RecieveMessage("System", $"{connection.UserName} has joined the chat.");
        }
 
        public async Task SendMessage(SendMessageRequest request)
        {
            var userId = Context.UserIdentifier ?? throw new ArgumentNullException();

            var username = Context.User!.GetUsername() ?? throw new ArgumentNullException();

            Guid.TryParse(request.ChatId, out var resultId);

            var message = await sender.Send(new AddMessageCommand(resultId, Context.ConnectionId, userId, username, request.Message));            

            await Clients
                .Group(request.ChatId)
                .RecieveMessage(username, request.Message);
        }
    }
}
