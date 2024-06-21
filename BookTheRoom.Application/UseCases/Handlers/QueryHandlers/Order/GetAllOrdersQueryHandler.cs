using BookTheRoom.Application.UseCases.Queries.Order;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<Core.Entities.Order>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Orders.GetAll();
        }
    }
}
