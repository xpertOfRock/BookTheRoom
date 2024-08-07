using Core.Contracts;
using MediatR;

namespace Application.UseCases.Queries.Room
{
    public class GetHotelRoomsQuery : IRequest<List<Core.Entities.Room>>
    {
        public int HotelId { get; set; }
        public GetDataRequest Request { get; set; }
        public GetHotelRoomsQuery(int hotelId, GetDataRequest request)
        {
            HotelId = hotelId;
            Request = request;
        }
    }
}
