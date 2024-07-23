using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateHotelCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.Hotels.Add(new Core.Entities.Hotel
                {
                    Name = command.Name,
                    Description = command.Description,
                    Rating = command.Rating,
                    RoomsAmount = command.RoomsAmount,
                    HasPool = command.HasPool,
                    Address = command.Address
                }
            );

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
