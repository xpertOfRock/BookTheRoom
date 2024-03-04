using BookTheRoom.Application.Helpers;
using BookTheRoom.Application.Interfaces;
using Braintree;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BookTheRoom.Application.Services
{
    public class PaymentService : IBraintreeService
    {
        private readonly IOptions<BraintreeSettings> _config;

        public PaymentService(IOptions<BraintreeSettings> config)
        {
            _config = config;
        }


        public IBraintreeGateway CreateGateway()
        {
            var newGateway = new BraintreeGateway()
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = _config.Value.MerchantId,
                PublicKey = _config.Value.PublicKey,
                PrivateKey = _config.Value.PrivateKey
            };

            return newGateway;
        }

        public IBraintreeGateway GetGateway()
        {
            return CreateGateway();

        }
    }
}
