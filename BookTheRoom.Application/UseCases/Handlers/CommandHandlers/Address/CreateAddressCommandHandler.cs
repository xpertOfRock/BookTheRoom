using BookTheRoom.Application.UseCases.Commands.Address;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.CommandHandlers.Address
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateAddressCommandHandler(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Addresses.Add(new Core.ValueObjects.Address
            {
                Country = request.Address.Country,
                City = request.Address.City,
                StreetOrDistrict = request.Address.StreetOrDistrict,
                Index = request.Address.Index
            });

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
