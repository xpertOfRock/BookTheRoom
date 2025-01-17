using Application.UseCases.Queries.Hotel;

namespace Application.UseCases.Handlers.QueryHandlers.Hotel
{
    public class GetHotelQueryHandler : IQueryHandler<GetHotelQuery, Core.Entities.Hotel>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetHotelQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Core.Entities.Hotel> Handle(GetHotelQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Hotels.GetById(request.Id);
        }
    }
}
