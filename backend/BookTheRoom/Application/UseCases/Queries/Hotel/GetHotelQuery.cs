using MediatR;

namespace Application.UseCases.Queries.Hotel
{
    public class GetHotelQuery : IRequest<Core.Entities.Hotel>
    {
        public int Id { get; set; }
        public GetHotelQuery(int id)
        {
            Id = id;
        }
    }
}
