using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    [Authorize]
    public class ChatHub(IUnitOfWork unitOfWork) : Hub
    {
  

        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("Admin"))
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            await base.OnConnectedAsync();
        }

        public async Task JoinChat(Guid chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task SendMessage(Guid chatId, string text)
        {
            var userId = Context.UserIdentifier!;

            var username = Context.User?.Identity?.Name ?? "Unknown";

            var message = await unitOfWork.Chats.AddMessageAsync(chatId, userId, username, text);

            await Clients.Group(chatId.ToString())
                         .SendAsync("ReceiveMessage", message);

            await Clients.Group("Admins")
                         .SendAsync("NewSupportMessage", new
                         {
                             ChatId = chatId,
                             From = username,
                             Preview = text.Length > 50 ? text[..50] + "…" : text
                         });
        }
    }
}
