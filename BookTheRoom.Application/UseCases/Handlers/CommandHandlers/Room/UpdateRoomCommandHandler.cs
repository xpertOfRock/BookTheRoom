using BookTheRoom.Application.UseCases.Commands.Room;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.CommandHandlers.Room
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Rooms.Update(request.Room);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
