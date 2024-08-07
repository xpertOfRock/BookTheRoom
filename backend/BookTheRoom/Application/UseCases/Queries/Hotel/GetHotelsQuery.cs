using Core.Contracts;
using Core.Entities;
using MediatR;

namespace Application.UseCases.Queries.Hotel
{
    public class GetHotelsQuery : IRequest<List<Core.Entities.Hotel>>
    {
        public GetDataRequest Request { get; set; }
        public GetHotelsQuery(GetDataRequest request)
        {
            Request = request;
        }
    }
}