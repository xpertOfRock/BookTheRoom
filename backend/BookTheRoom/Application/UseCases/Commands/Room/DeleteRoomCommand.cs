using Application.UseCases.Abstractions;

namespace Application.UseCases.Commands.Room
{
    public class DeleteRoomCommand : ICommand<IResult>
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
