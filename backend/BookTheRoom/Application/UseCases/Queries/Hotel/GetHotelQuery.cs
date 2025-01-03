using Application.UseCases.Abstractions;

namespace Application.UseCases.Queries.Hotel
{
    public class GetHotelQuery : IQuery<Core.Entities.Hotel>
    {
        public int Id { get; set; }
        public GetHotelQuery(int id)
        {
            Id = id;
        }
    }
}
