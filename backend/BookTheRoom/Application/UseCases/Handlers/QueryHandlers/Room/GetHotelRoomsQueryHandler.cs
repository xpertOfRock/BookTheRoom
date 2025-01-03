using Application.UseCases.Abstractions;
using Application.UseCases.Queries.Room;

namespace Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetHotelRoomsQueryHandler : IQueryHandler<GetHotelRoomsQuery, List<Core.Entities.Room>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetHotelRoomsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Room>> Handle(GetHotelRoomsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Rooms.GetAll(request.HotelId, request.Request);
        }
    }
}
