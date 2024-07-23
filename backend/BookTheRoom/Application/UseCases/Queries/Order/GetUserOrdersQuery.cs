using MediatR;

namespace Application.UseCases.Queries.Order
{
    public class GetUserOrdersQuery : IRequest<List<Core.Entities.Order>>
    {
    }
}
