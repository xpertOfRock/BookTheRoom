using BookTheRoom.Application.UseCases.Commands.Hotel;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Hotels.Delete(request.Hotel);
            _unitOfWork.Addresses.Delete(request.Hotel.Address);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
