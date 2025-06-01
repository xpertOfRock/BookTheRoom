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

            var username = Context.User!.GetUsername() ?? throw new ArgumentNullException();

            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatId);
        }
 
        public async Task SendMessage(SendMessageRequest request)
        {
            var userId = Context.UserIdentifier ?? throw new ArgumentNullException();

            var username = Context.User!.GetUsername() ?? throw new ArgumentNullException();

            Guid.TryParse(request.ChatId, out var resultId);

            var message = await sender.Send(new AddMessageCommand(resultId, Context.ConnectionId, userId, username, request.Message.Trim()));            

            await Clients
                .Group(request.ChatId)
                .ReceiveMessage(message.Id ,userId, username, request.Message.Trim(), message.CreatedAt, Context.ConnectionId);
        }
    }
}
