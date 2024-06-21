using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Room
{
    public class GetRoomByNumberQuery : IRequest<Core.Entities.Room>
    {
        public int Number { get; set; }
        public int HotelId { get; set; }
        public GetRoomByNumberQuery(int hotelId, int number)
        {
            Number = number;
            HotelId = hotelId;
        }
    }
}
