using BookTheRoom.Application.UseCases.Commands.Hotel;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Hotels.Add(new Core.Entities.Hotel
            {
                Name = request.Hotel.Name,
                Description = request.Hotel.Description,
                NumberOfRooms = request.Hotel.NumberOfRooms,
                Address = request.Hotel.Address,
                HasPool = request.Hotel.HasPool,
                ImagesURL = request.Hotel.ImagesURL,
                PreviewURL = request.Hotel.PreviewURL,
                Rating = request.Hotel.Rating,
                Rooms = request.Hotel.Rooms
            });

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
