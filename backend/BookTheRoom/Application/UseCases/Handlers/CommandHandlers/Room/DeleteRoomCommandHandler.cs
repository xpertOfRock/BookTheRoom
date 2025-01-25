using Application.UseCases.Commands.Room;

namespace Application.UseCases.Handlers.CommandHandlers.Room
{
    public class DeleteRoomCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteRoomCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await unitOfWork.Rooms.Delete(command.HotelId, command.Number);

                if (!result.IsSuccess)
                {
                    await unitOfWork.RollbackAsync();
                    return result;
                }

                await unitOfWork.SaveChangesAsync();

                await unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the room.", ex);
            }
        }
    }
}
