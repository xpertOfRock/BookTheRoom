using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Hotel
{
    public class GetAllHotelsQuery : IRequest<List<Core.Entities.Hotel>>
    {

    }
}
