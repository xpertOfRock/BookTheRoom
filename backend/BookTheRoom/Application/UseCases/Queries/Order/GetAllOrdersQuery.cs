using MediatR;

namespace Application.UseCases.Queries.Order
{
    public class GetAllOrdersQuery : IRequest<List<Core.Entities.Order>>
    {
    }
}
