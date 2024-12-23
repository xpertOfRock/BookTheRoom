namespace Application.UseCases.Queries.Room
{
    public class GetHotelRoomsQuery : IRequest<List<Core.Entities.Room>>
    {
        public int HotelId { get; set; }
        public GetRoomsRequest Request { get; set; }
        public GetHotelRoomsQuery(int hotelId, GetRoomsRequest request)
        {
            HotelId = hotelId;
            Request = request;
        }
    }
}
