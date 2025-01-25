using Application.UseCases.Queries.Hotel;

namespace Application.UseCases.Handlers.QueryHandlers.Hotel
{
    public class GetHotelsQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetHotelsQuery, List<Core.Entities.Hotel>>
    {
        public async Task<List<Core.Entities.Hotel>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Hotels.GetAll(request.Filter);
        }
    }
}
