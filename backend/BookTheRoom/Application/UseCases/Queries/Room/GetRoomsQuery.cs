using MediatR;

namespace Application.UseCases.Queries.Room
{
    public class GetRoomsQuery : IRequest<List<Core.Entities.Room>>
    {

    }
}
