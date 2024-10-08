﻿using Application.Interfaces;
using Application.UseCases.Queries.Order;
using MediatR;

namespace Application.UseCases.Handlers.QueryHandlers.Order
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
