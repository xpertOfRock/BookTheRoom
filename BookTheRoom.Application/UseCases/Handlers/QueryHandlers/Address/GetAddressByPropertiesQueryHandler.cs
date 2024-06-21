using BookTheRoom.Application.UseCases.Queries.Address;
using BookTheRoom.Core.ValueObjects;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Address
{
    public class GetAddressByPropertiesQueryHandler : IRequestHandler<GetAddressByPropertiesQuery, Core.ValueObjects.Address>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAddressByPropertiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Core.ValueObjects.Address> Handle(GetAddressByPropertiesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Addresses.GetAddress(request.Country, request.City, request.StreetOrDistrict, request.Index);
        }
    }
}
