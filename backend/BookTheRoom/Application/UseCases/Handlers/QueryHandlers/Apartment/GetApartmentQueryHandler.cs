using Application.UseCases.Queries.Apartment;

namespace Application.UseCases.Handlers.QueryHandlers.Apartment
{
    internal class GetApartmentQueryHandler : IQueryHandler<GetApartmentQuery, Core.Entities.Apartment>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetApartmentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Core.Entities.Apartment> Handle(GetApartmentQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Apartments.GetById(request.Id);
        }
    }
}
