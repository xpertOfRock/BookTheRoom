using Application.UseCases.Queries.Order;

namespace Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetAllOrdersQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetAllOrdersQuery, List<Core.Entities.Order>>
    {
        public async Task<List<Core.Entities.Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Orders.GetAll(new GetOrdersRequest(null, null, null));
        }
    }
}
