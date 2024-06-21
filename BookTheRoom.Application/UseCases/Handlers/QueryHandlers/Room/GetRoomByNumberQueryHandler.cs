using BookTheRoom.Application.UseCases.Queries.Room;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetRoomByNumberQueryHandler : IRequestHandler<GetRoomByNumberQuery, Core.Entities.Room>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRoomByNumberQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Core.Entities.Room> Handle(GetRoomByNumberQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Rooms.GetByNumber(request.HotelId, request.Number);
        }
    }
}
