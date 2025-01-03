using Application.UseCases.Abstractions;

namespace Application.UseCases.Queries.Order
{
    public class GetActiveOrdersQuery : IQuery<List<Core.Entities.Order>?>
    {
    }
}
