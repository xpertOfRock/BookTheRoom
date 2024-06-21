using BookTheRoom.Application.UseCases.Queries.Order;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetActiveOrdersQueryHandler : IRequestHandler<GetActiveOrdersQuery, List<Core.Entities.Order>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetActiveOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Core.Entities.Order>> Handle(GetActiveOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Orders.GetActiveOrders();
        }
    }
}
