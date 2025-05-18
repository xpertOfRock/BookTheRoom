using Application.UseCases.Commands.Chat;

namespace Application.UseCases.Handlers.CommandHandlers.Chat
{
    public class CreateChatCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateChatCommand, Core.Entities.Chat>
    {
        public async Task<Core.Entities.Chat> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.Chats.CreateChatAsync();
        }
    }
}
