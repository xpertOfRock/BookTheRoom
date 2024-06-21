using BookTheRoom.Application.UseCases.Commands.Address;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.CommandHandlers.Address
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteAddressCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Addresses.Delete(request.Address);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
