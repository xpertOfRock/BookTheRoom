using Application.UseCases.Queries.Hotel;

namespace Application.UseCases.Handlers.QueryHandlers.Hotel
{
    public class GetHotelsQueryHandler : IQueryHandler<GetHotelsQuery, List<Core.Entities.Hotel>>
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
