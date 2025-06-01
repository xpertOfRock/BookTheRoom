using Application.UseCases.Commands.Chat;

namespace Application.UseCases.Handlers.CommandHandlers.Chat
{
    public class CreateChatCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateChatCommand, Core.Entities.Chat>
    {
        public async Task<Core.Entities.Chat> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {                
                var result = await unitOfWork.Chats.CreateChat(request.UsersId, request.ApartmentId);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                await unitOfWork.CommitAsync();
                
                if(request.ApartmentId is not null)
                {
                    var apartment = await unitOfWork.Apartments.GetById((int)request.ApartmentId);

                    await unitOfWork.Apartments.UpdateCache(apartment);
                }

                return result;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the comment.", ex);
            }
            
        }
    }
}
