using BookTheRoom.Core.Entities;
using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Hotel
{
    public class UpdateHotelCommand : IRequest<Unit>
    {
        public Core.Entities.Hotel Hotel { get; set; }
        public UpdateHotelCommand(Core.Entities.Hotel hotel)
        {
            Hotel = hotel;
        }
    }
}
