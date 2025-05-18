using Application.UseCases.Queries.Apartment;

namespace Application.UseCases.Handlers.QueryHandlers.Apartment
{
    public class GetApartmentsQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetApartmentsQuery, List<Core.Entities.Apartment>>
    {
        public async Task<List<Core.Entities.Apartment>> Handle(GetApartmentsQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Apartments.GetAll(request.Filter, cancellationToken);
        }
    }
}
