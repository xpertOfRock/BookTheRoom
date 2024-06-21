using BookTheRoom.Application.UseCases.Queries.Room;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, List<Core.Entities.Room>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllRoomsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Room>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Rooms.GetAll();
        }
    }
}
