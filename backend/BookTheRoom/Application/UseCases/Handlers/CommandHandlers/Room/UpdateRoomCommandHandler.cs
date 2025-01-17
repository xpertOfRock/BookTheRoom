using Application.UseCases.Commands.Room;

namespace Application.UseCases.Handlers.CommandHandlers.Room
{
    public class UpdateRoomCommandHandler : ICommandHandler<UpdateRoomCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IResult> Handle(UpdateRoomCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var result = await _unitOfWork.Rooms.Update(command.HotelId, command.Number, command.Request);

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
