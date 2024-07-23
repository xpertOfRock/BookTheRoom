using Braintree;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        IBraintreeGateway CreateGateway();
        IBraintreeGateway GetGateway();
    }
}
