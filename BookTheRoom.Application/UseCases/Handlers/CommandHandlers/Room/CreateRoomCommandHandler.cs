using BookTheRoom.Application.UseCases.Commands.Room;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.CommandHandlers.Room
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Rooms.Add(request.Room);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
