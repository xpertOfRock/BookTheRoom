using Application.ExternalServices;
using Application.Settings;

namespace Tests.External_Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IOptions<BraintreeSettings>> _optionsMock;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            _optionsMock = new Mock<IOptions<BraintreeSettings>>();
            _optionsMock.Setup(x => x.Value).Returns(new BraintreeSettings
            {
                MerchantId = "your_merchant_id",
                PublicKey = "your_public_key",
                PrivateKey = "your_private_key"
            });

            _paymentService = new PaymentService(_optionsMock.Object);
        }

        [Fact]
        public void CreateGateway_ReturnsCorrectGateway()
        {
            var gateway = _paymentService.CreateGateway();

            Assert.NotNull(gateway);
            Assert.Equal(Braintree.Environment.SANDBOX, gateway.Environment);
            Assert.Equal(_optionsMock.Object.Value.MerchantId, gateway.MerchantId);
            Assert.Equal(_optionsMock.Object.Value.PublicKey, gateway.PublicKey);
            Assert.Equal(_optionsMock.Object.Value.PrivateKey, gateway.PrivateKey);
        }
    }
}
