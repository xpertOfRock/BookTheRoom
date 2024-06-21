using BookTheRoom.Application.UseCases.Commands.Address;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.CommandHandlers.Address
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateAddressCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Addresses.Update(request.Address);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
