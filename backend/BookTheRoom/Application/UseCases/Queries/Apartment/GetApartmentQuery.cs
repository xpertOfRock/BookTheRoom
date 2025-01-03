namespace Application.UseCases.Queries.Apartment
{
    public class GetApartmentQuery : IQuery<Core.Entities.Apartment>
    {
        public int Id { get; set; }
        public GetApartmentQuery(int id)
        {
            Id = id;
        }
    }
}
