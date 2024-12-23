using Application.UseCases.Queries.Apartment;

namespace Application.UseCases.Handlers.QueryHandlers.Apartment
{
    public class GetApartmentsQueryHandler : IRequestHandler<GetApartmentsQuery, List<Core.Entities.Apartment>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetApartmentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Apartment>> Handle(GetApartmentsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Apartments.GetAll(request.Filter);
        }
    }
}
