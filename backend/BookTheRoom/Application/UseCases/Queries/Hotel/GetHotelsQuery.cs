using Application.UseCases.Abstractions;

namespace Application.UseCases.Queries.Hotel
{
    public class GetHotelsQuery : IQuery<List<Core.Entities.Hotel>>
    {
        public GetHotelsRequest Filter { get; set; }
        public GetHotelsQuery(GetHotelsRequest request)
        {
            Filter = request;
        }
    }
}