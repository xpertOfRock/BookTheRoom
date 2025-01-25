using Application.UseCases.Queries.Apartment;

namespace Application.UseCases.Handlers.QueryHandlers.Apartment
{
    internal class GetApartmentQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetApartmentQuery, Core.Entities.Apartment>
    {
        public async Task<Core.Entities.Apartment> Handle(GetApartmentQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Apartments.GetById(request.Id);
        }
    }
}
