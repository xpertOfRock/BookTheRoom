using MediatR;

namespace Application.UseCases.Queries.Apartment
{
    public class GetApartmentQuery : IRequest<Core.Entities.Apartment>
    {
        public int Id { get; set; }
        public GetApartmentQuery(int id)
        {
            Id = id;
        }
    }
}
