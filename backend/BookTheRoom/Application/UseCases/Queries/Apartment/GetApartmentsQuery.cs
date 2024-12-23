namespace Application.UseCases.Queries.Apartment
{
    public class GetApartmentsQuery : IRequest<List<Core.Entities.Apartment>>
    {
        public GetApartmentsRequest Filter { get; set; }
        public GetApartmentsQuery(GetApartmentsRequest request)
        {
            Filter = request;
        }
    }
}
