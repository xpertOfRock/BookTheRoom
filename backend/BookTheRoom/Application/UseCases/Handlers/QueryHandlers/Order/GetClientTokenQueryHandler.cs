using Application.UseCases.Queries.Order;

namespace Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetClientTokenQueryHandler(IPaymentService paymentSerivce) : IQueryHandler<GetClientTokenQuery, string>
    {
        public async Task<string> Handle(GetClientTokenQuery request, CancellationToken cancellationToken)
        {
            var gateway = paymentSerivce.CreateGateway();

            return await gateway.ClientToken.GenerateAsync();
        }
    }
}
