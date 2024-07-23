using Core.Contracts;
using Core.Entities;
using MediatR;

namespace Application.UseCases.Queries.Hotel
{
    public class GetHotelsQuery : IRequest<List<Core.Entities.Hotel>>
    {
        public GetHotelsRequest Request { get; set; }
        public GetHotelsQuery(GetHotelsRequest request)
        {
            Request = request;
        }
    }
}