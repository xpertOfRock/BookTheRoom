using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Order
{
    public class GetActiveOrdersQuery : IRequest<List<Core.Entities.Order>>
    {

    }
}
