using Braintree;

namespace Application.Interfaces
{
    /// <summary>
    /// Service for managing payment gateway operations.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Creates a new instance of the Braintree payment gateway.
        /// </summary>
        /// <returns>An instance of <see cref="IBraintreeGateway"/>.</returns>
        IBraintreeGateway CreateGateway();

        /// <summary>
        /// Retrieves the existing instance of the Braintree payment gateway.
        /// </summary>
        /// <returns>An instance of <see cref="IBraintreeGateway"/>.</returns>
        IBraintreeGateway GetGateway();
    }
}
