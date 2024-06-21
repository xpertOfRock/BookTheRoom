using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Room
{
    public class GetAllRoomsQuery : IRequest<List<Core.Entities.Room>>
    {

    }
}
