using Application.UseCases.Abstractions;

namespace Application.UseCases.Queries.Order
{
    public class GetAllOrdersQuery : IQuery<List<Core.Entities.Order>?>
    {
    }
}
