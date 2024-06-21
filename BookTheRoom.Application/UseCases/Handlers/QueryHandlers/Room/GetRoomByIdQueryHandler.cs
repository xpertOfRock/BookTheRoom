using BookTheRoom.Application.UseCases.Queries.Room;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, Core.Entities.Room>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRoomByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Core.Entities.Room> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Rooms.GetById(request.RoomId);
        }
    }
}
