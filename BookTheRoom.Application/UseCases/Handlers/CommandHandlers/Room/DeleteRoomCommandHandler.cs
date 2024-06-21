using BookTheRoom.Application.UseCases.Commands.Room;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.CommandHandlers.Room
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Rooms.Delete(request.Room);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
