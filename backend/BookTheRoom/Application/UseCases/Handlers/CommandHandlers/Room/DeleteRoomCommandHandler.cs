using Application.UseCases.Commands.Room;

namespace Application.UseCases.Handlers.CommandHandlers.Room
{
    public class DeleteRoomCommandHandler : ICommandHandler<DeleteRoomCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IResult> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.Rooms.Delete(command.HotelId, command.Number);

                if (!result.IsSuccess)
                {
                    await _unitOfWork.RollbackAsync();
                    return result;
                }

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the room.", ex);
            }
        }
    }
}
