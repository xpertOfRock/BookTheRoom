using BookTheRoom.Application.UseCases.Queries.Address;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Address
{
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, List<Core.ValueObjects.Address>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllAddressesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.ValueObjects.Address>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Addresses.GetAllAsync();
        }
    }
}
