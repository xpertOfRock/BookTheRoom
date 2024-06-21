using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Room
{
    public class GetRoomsByHotelQuery : IRequest<List<Core.Entities.Room>>
    {
        public int HotelId { get; set; }
        public GetRoomsByHotelQuery(int hotelId)
        {
            HotelId = hotelId;
        }
    }
}
