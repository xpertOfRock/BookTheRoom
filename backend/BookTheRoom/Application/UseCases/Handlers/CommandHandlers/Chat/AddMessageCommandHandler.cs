using Application.UseCases.Commands.Chat;
using Core.Entities;

namespace Application.UseCases.Handlers.CommandHandlers.Chat
{
    public class AddMessageCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<AddMessageCommand, ChatMessage>
    {
        public async Task<ChatMessage> Handle(AddMessageCommand command, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var chat = await unitOfWork.Chats.GetChatById(command.ChatId, cancellationToken);

                var message = new ChatMessage
                {
                    ChatId = command.ChatId,
                    ConnectionId = command.ConnectionId,
                    UserId = command.UserId,
                    UserName = command.UserName,
                    Message = command.Message,
                    CreatedAt = DateTime.UtcNow,
                };

                await unitOfWork.Chats.AddMessage(message);

                await unitOfWork.SaveChangesAsync();

                await unitOfWork.CommitAsync();

                chat.Messages?.Add(message);

                await unitOfWork.Chats.UpdateCache(chat);

                return message;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the comment.", ex);
            }
        }
    }
}
