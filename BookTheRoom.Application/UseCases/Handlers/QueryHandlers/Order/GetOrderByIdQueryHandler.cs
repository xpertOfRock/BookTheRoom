using BookTheRoom.Application.UseCases.Queries.Order;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Core.Entities.Order>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Core.Entities.Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Orders.GetById(request.Id);
        }
    }
}
