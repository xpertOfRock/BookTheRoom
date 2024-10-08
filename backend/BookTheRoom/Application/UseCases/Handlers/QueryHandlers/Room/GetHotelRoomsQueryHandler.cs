﻿using Application.Interfaces;
using Application.UseCases.Queries.Room;
using MediatR;

namespace Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetHotelRoomsQueryHandler : IRequestHandler<GetHotelRoomsQuery, List<Core.Entities.Room>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetHotelRoomsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Room>> Handle(GetHotelRoomsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Rooms.GetAll(request.HotelId, request.Request);
        }
    }
}
