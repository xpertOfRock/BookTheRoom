using Application.UseCases.Queries.Apartment;

namespace Application.UseCases.Handlers.QueryHandlers.Apartment
{
    public class GetUsersApartmentsQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetUsersApartmentsQuery, List<Core.Entities.Apartment>>
    {
        public async Task<List<Core.Entities.Apartment>> Handle(GetUsersApartmentsQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Apartments.GetAllUsersApartments(request.UserId, request.Filter);
        }
    }
}
