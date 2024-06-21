using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Hotel
{
    public class CreateHotelCommand : IRequest<Unit>
    {
        public Core.Entities.Hotel Hotel { get; set; }

        public CreateHotelCommand(Core.Entities.Hotel hotel)
        {
            Hotel = hotel;
        }
    }
}
