using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Order
{
    public class GetUserOrdersQuery : IRequest<List<Core.Entities.Order>>
    {
        public string UserId { get; set; }
        public GetUserOrdersQuery(string userId)
        {
            UserId = userId;
        }
    }
}
