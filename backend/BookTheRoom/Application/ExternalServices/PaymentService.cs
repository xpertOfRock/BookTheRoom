using Application.Interfaces;
using Application.Settings;
using Braintree;
using Microsoft.Extensions.Options;

namespace Application.ExternalServices
{
    public class PaymentService : IPaymentService
    {
        private readonly BraintreeSettings _options;
        public PaymentService(IOptions<BraintreeSettings> options)
        {
            _options = options.Value;
        }
        public IBraintreeGateway CreateGateway()
        {
            var newGateway = new BraintreeGateway()
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = _options.MerchantId,
                PublicKey = _options.PublicKey,
                PrivateKey = _options.PrivateKey
            };

            return newGateway;
        }

        public IBraintreeGateway GetGateway()
        {
            return CreateGateway();
        }
    }
}
