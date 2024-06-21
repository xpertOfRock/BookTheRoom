using BookTheRoom.Application.UseCases.Queries.Hotel;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Hotel
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, Core.Entities.Hotel>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetHotelByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Core.Entities.Hotel> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Hotels.GetById(request.Id);
        }
    }
}
