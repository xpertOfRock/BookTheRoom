namespace Application.UseCases.Queries.Order
{
    public class GetAllOrdersQuery : IQuery<List<Core.Entities.Order>>
    {
        public GetOrdersRequest Filter { get; set; }
        public GetAllOrdersQuery(GetOrdersRequest request)
        {
            Filter = request;
        }
    }
}
