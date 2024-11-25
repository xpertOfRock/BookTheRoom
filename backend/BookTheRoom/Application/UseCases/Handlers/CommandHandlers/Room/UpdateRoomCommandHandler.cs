using Application.Interfaces;
using Application.UseCases.Commands.Room;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Room
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateRoomCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Rooms.Update(command.HotelId, command.Number, command.Request);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the room.", ex);
            }
            return Unit.Value;
        }
    }
}
