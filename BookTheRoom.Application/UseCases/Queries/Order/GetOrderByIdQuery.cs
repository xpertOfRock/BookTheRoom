using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Order
{
    public class GetOrderByIdQuery : IRequest<Core.Entities.Order>
    {
        public int Id { get; set; }
        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }
    }
}
