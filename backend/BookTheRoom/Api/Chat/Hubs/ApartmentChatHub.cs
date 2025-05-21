using Api.Chat.Interfaces;
using Api.Chat.Models;
using Api.Extensions;
using Application.UseCases.Commands.Chat;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task CreateChat(List<string> userIds, int? apartmentId = null)
        {
            var result = await sender.Send(new CreateChatCommand(userIds, apartmentId));           

            await Groups.AddToGroupAsync(Context.ConnectionId, result.Id.ToString());
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
        //public async Task SendMessage(Guid chatId, string text)
        //{
        //    var userId = Context.UserIdentifier!;

        //    var username = Context.User?.Identity?.Name ?? "Unknown";

        //    var message = await unitOfWork.Chats.AddMessage(chatId, userId, username, text);

        //    await Clients.Group(chatId.ToString())
        //                 .SendAsync("ReceiveMessage", message);

        //    await Clients.Group("Admins")
        //                 .SendAsync("NewSupportMessage", new
        //                 {
        //                     ChatId = chatId,
        //                     From = username,
        //                     Preview = text.Length > 50 ? text[..50] + "…" : text
        //                 });
        //}
    }
}
