using Application.UseCases.Abstractions;
using Application.UseCases.Queries.Order;

namespace Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetActiveOrdersQueryHandler : IQueryHandler<GetActiveOrdersQuery, List<Core.Entities.Order>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetActiveOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Order>?> Handle(GetActiveOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Orders.GetActiveOrders();
        }
    }
}
