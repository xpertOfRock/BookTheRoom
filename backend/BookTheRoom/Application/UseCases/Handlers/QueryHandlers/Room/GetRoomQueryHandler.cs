using Application.UseCases.Queries.Room;
using Core.Entities;

namespace Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetRoomQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetRoomQuery, Core.Entities.Room>
    {
        public async Task<Core.Entities.Room> Handle(GetRoomQuery request, CancellationToken cancellationToken)
        {           
            return await unitOfWork.Rooms.GetById(request.HotelId, request.Number);
        }
    }
}
