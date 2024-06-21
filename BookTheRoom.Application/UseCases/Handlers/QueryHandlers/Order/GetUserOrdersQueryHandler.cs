using BookTheRoom.Application.UseCases.Queries.Order;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, List<Core.Entities.Order>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Order>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Orders.GetUserOrders(request.UserId);
        }
    }
}
