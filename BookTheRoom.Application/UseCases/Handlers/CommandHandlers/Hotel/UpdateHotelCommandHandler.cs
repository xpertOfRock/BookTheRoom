using BookTheRoom.Application.UseCases.Commands.Hotel;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Hotels.Update(request.Hotel);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
