using BookTheRoom.Application.UseCases.Queries.Hotel;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;

namespace BookTheRoom.Application.UseCases.Handlers.QueryHandlers.Hotel
{
    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, List<Core.Entities.Hotel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllHotelsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Core.Entities.Hotel>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Hotels.GetAll();
        }
    }
}
