using MediatR;

namespace Application.UseCases.Queries.Order
{
    public class GetActiveOrdersQuery : IRequest<List<Core.Entities.Order>>
    {
    }
}
