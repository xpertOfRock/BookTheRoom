using Application.UseCases.Abstractions;

namespace Application.UseCases.Queries.Apartment
{
    public class GetUsersApartmentsQuery : IQuery<List<Core.Entities.Apartment>>
    {
        public string UserId { get; set; }
        public GetApartmentsRequest Filter { get; set; }
        public GetUsersApartmentsQuery(string userId, GetApartmentsRequest request)
        {
            UserId = userId;
            Filter = request;
        }
    }
}
