using Application.Interfaces;
using Application.UseCases.Queries.Apartment;
using MediatR;

namespace Application.UseCases.Handlers.QueryHandlers.Apartment
{
    public class GetUsersApartmentsQueryHandler : IRequestHandler<GetUsersApartmentsQuery, List<Core.Entities.Apartment>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUsersApartmentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Apartment>> Handle(GetUsersApartmentsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Apartments.GetAllUsersApartments(request.UserId, request.Filter);
        }
    }
}
