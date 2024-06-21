using BookTheRoom.Application.UseCases.Queries.Room;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetRoomsByHotelQueryHandler : IRequestHandler<GetRoomsByHotelQuery, List<Core.Entities.Room>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRoomsByHotelQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Core.Entities.Room>> Handle(GetRoomsByHotelQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Rooms.GetAllRoomsByHotel(request.HotelId);
        }
    }
}
