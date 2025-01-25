using Application.UseCases.Queries.Order;

namespace Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetExpiredOrdersQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetExpiredOrdersQuery, List<Core.Entities.Order>>
    {
        public async Task<List<Core.Entities.Order>?> Handle(GetExpiredOrdersQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Orders.GetExpiredOrders();
        }
    }
}
