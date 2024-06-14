using Braintree;

namespace BookTheRoom.Application.Interfaces
{
    public interface IPaymentService
    {
        IBraintreeGateway CreateGateway();
        IBraintreeGateway GetGateway();
    }
}
