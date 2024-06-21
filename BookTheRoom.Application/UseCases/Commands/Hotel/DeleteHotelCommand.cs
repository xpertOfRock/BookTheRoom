using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Hotel
{
    public class DeleteHotelCommand : IRequest<Unit>
    {
        public Core.Entities.Hotel Hotel { get; set; }
        public DeleteHotelCommand(Core.Entities.Hotel hotel)
        {
            Hotel = hotel;
        }
    }
}
