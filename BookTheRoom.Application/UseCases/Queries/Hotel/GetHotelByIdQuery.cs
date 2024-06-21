using BookTheRoom.Core.Entities;
using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Hotel
{
    public class GetHotelByIdQuery : IRequest<Core.Entities.Hotel>
    {
        public int Id { get; set; }
        public GetHotelByIdQuery(int id)
        {
            Id = id;
        }
    }
}
