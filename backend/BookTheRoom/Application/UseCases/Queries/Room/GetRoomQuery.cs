using MediatR;

namespace Application.UseCases.Queries.Room
{
    public class GetRoomQuery : IRequest<Core.Entities.Room>
    {
        public int HotelId { get; set; }
        public int Number { get; set; }
        public GetRoomQuery(int hotelId, int number)
        {
            HotelId = hotelId;
            Number = number;
        }
    }
}
