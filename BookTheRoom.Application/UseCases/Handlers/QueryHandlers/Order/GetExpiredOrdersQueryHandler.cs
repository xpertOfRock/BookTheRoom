﻿using BookTheRoom.Application.UseCases.Queries.Order;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetExpiredOrdersQueryHandler : IRequestHandler<GetExpiredOrdersQuery, List<Core.Entities.Order>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetExpiredOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Order>> Handle(GetExpiredOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Orders.GetExpiredOrders();
        }
    }
}
