using Application.Interfaces;
using Application.UseCases.Queries.Hotel;
using MediatR;

namespace Application.UseCases.Handlers.QueryHandlers.Hotel
{
    public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, List<Core.Entities.Hotel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetHotelsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Core.Entities.Hotel>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Hotels.GetAll(request.Filter);
        }
    }
}
