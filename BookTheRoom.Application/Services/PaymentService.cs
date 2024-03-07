using BookTheRoom.Application.DTO;
using BookTheRoom.Application.Interfaces;
using Braintree;
using Microsoft.Extensions.Options;

namespace BookTheRoom.Application.Services
{
    public class PaymentService : IBraintreeService
    {
        private readonly IOptions<BraintreeSettings> _config;

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
