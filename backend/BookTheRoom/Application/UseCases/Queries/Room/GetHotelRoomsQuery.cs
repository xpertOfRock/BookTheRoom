using Application.UseCases.Abstractions;

namespace Application.UseCases.Queries.Room
{
    public class GetHotelRoomsQuery : IQuery<List<Core.Entities.Room>>
    {
        public int HotelId { get; set; }
        public GetRoomsRequest Request { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public GetHotelRoomsQuery(int hotelId, GetRoomsRequest request)
        {
            HotelId = hotelId;
            Request = request;
            CheckIn = request.CheckIn;
            CheckOut = request.CheckOut;
        }
    }
}
