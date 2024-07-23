using Application.Interfaces;
using Application.UseCases.Commands.Room;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Room
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.Rooms.Delete(command.HotelId, command.Number);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
