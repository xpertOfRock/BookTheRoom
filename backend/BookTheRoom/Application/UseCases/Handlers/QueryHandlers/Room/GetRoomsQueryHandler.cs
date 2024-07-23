using Application.UseCases.Queries.Room;
using MediatR;

namespace Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, List<Core.Entities.Room>>
    {
        public GetRoomsQueryHandler()
        {
            
        }
        public Task<List<Core.Entities.Room>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
