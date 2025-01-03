using Application.UseCases.Abstractions;

namespace Application.UseCases.Queries.Order
{
    public class GetUserOrdersQuery : IQuery<List<Core.Entities.Order>?>
    {
    }
}
