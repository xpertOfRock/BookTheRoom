using Application.UseCases.Queries.Order;

namespace Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetActiveOrdersQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetActiveOrdersQuery, List<Core.Entities.Order>?>
    {
        public async Task<List<Core.Entities.Order>?> Handle(GetActiveOrdersQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Orders.GetActiveOrders();
        }
    }
}
