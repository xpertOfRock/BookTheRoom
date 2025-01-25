using Application.UseCases.Queries.Room;

namespace Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetRoomsQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetRoomsQuery, List<Core.Entities.Room>>
    {
        public async Task<List<Core.Entities.Room>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Rooms.GetAllRooms();
        }
    }
}
