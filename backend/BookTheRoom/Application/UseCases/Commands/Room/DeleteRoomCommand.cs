using MediatR;

namespace Application.UseCases.Commands.Room
{
    public class DeleteRoomCommand : IRequest<Unit>
    {
        public int HotelId { get; set; }
        public int Number { get; set; }
        public DeleteRoomCommand(int hotelId, int number)
        {
            HotelId = hotelId;
            Number = number;
        }
    }
}
