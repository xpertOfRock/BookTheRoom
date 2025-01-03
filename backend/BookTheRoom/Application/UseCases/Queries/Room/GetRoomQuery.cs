using Application.UseCases.Abstractions;
using MediatR;

namespace Application.UseCases.Queries.Room
{
    public class GetRoomQuery : IQuery<Core.Entities.Room>
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
