using Application.Interfaces;
using Application.UseCases.Queries.Apartment;
using MediatR;

namespace Application.UseCases.Handlers.QueryHandlers.Apartment
{
    internal class GetApartmentQueryHandler : IRequestHandler<GetApartmentQuery, Core.Entities.Apartment>
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
