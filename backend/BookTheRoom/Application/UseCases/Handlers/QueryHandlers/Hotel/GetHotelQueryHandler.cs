using Application.UseCases.Queries.Hotel;

namespace Application.UseCases.Handlers.QueryHandlers.Hotel
{
    public class GetHotelQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetHotelQuery, Core.Entities.Hotel>
    {
        public async Task<Core.Entities.Hotel> Handle(GetHotelQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Hotels.GetById(request.Id);
        }
    }
}
