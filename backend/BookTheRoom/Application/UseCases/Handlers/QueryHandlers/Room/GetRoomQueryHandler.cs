using Application.UseCases.Queries.Room;

namespace Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, Core.Entities.Room>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRoomQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Core.Entities.Room> Handle(GetRoomQuery request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms.GetById(request.HotelId, request.Number);

            if (room == null)
            {
                return null;
            }

            return room;
        }
    }
}
