using Application.Interfaces;
using Application.UseCases.Queries.Room;
using MediatR;

namespace Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, List<Core.Entities.Room>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRoomsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Room>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Rooms.GetAllRooms();
        }
    }
}
