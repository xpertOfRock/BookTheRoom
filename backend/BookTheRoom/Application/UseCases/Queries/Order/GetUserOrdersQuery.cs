using Application.DTOs;

namespace Application.UseCases.Queries.Order
{
    public class GetUserOrdersQuery : IQuery<List<UserOrdersDTO>>
    {
        public string UserId { get; }
        public GetOrdersRequest Filters { get; }
        public GetUserOrdersQuery(string userId, GetOrdersRequest request)
        {
            UserId = userId;
            Filters = request;
        }
    }
}
