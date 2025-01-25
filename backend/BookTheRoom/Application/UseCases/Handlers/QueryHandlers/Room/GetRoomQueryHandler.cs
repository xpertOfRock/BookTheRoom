using Application.UseCases.Queries.Room;

namespace Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetRoomQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetRoomQuery, Core.Entities.Room>
    {
        public async Task<Core.Entities.Room> Handle(GetRoomQuery request, CancellationToken cancellationToken)
        {
            var room = await unitOfWork.Rooms.GetById(request.HotelId, request.Number);

            if (room == null)
            {
                return null;
            }

            return room;
        }
    }
}
