namespace Application.UseCases.Commands.Room
{
    public class UpdateRoomCommand : ICommand<IResult>
    {
        public int HotelId { get; set; }
        public int Number { get; set; }
        public UpdateRoomRequest Request { get; set; }
        public UpdateRoomCommand(int hotelId, int number, UpdateRoomRequest request)
        {
            HotelId = hotelId;
            Number = number;
            Request = request;
        }
    }
}
