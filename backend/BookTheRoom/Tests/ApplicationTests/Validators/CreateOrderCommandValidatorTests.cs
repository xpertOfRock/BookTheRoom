using Application.UseCases.Commands.Order;
using Application.UseCases.Validators.Order;
using Core.Contracts;

namespace Tests.ApplicationTests.Validators
{
    // Unit tests for CreateOrderCommandValidator.
    public class CreateOrderCommandValidatorTests
    {
        private readonly CreateOrderCommandValidator _validator = new();

        private static CreateOrderCommand ValidCommand() =>
            new CreateOrderCommand(
                hotelId: 1,
                number: 101,
                userId: "user-001",
                request: new CreateOrderRequest(
                    NonceFromClient: "nonce-123",
                    Email: "guest@example.com",
                    Number: "+380000000000",
                    FirstName: "John",
                    LastName: "Doe",
                    MinibarIncluded: false,
                    MealsIncluded: false,
                    CheckIn: DateTimeOffset.UtcNow.AddDays(1),
                    CheckOut: DateTimeOffset.UtcNow.AddDays(4)));

        [Fact]
        public void Validate_WithValidCommand_Passes()
        {
            var result = _validator.Validate(ValidCommand());
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_WithEmptyPhone_Fails(string? phone)
        {
            var cmd = new CreateOrderCommand(1, 101, "user-001",
                new CreateOrderRequest("nonce", "a@b.com", phone!,
                    "John", "Doe", false, false,
                    DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(2)));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Number"));
        }

        [Theory]
        [InlineData("not-an-email")]
        [InlineData("")]
        public void Validate_WithInvalidEmail_Fails(string email)
        {
            var cmd = new CreateOrderCommand(1, 101, "user-001",
                new CreateOrderRequest("nonce", email, "+380000000000",
                    "John", "Doe", false, false,
                    DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(2)));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Email"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_WithEmptyFirstName_Fails(string? firstName)
        {
            var cmd = new CreateOrderCommand(1, 101, "user-001",
                new CreateOrderRequest("nonce", "a@b.com", "+380000000000",
                    firstName!, "Doe", false, false,
                    DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(2)));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("FirstName"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_WithEmptyNonce_Fails(string? nonce)
        {
            var cmd = new CreateOrderCommand(1, 101, "user-001",
                new CreateOrderRequest(nonce!, "a@b.com", "+380000000000",
                    "John", "Doe", false, false,
                    DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(2)));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("NonceFromClient"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_WithEmptyUserId_Fails(string? userId)
        {
            var cmd = new CreateOrderCommand(1, 101, userId,
                new CreateOrderRequest("nonce", "a@b.com", "+380000000000",
                    "John", "Doe", false, false,
                    DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(2)));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "UserId");
        }

        [Fact]
        public void Validate_WithZeroHotelId_Fails()
        {
            var cmd = new CreateOrderCommand(0, 101, "user-001",
                new CreateOrderRequest("nonce", "a@b.com", "+380000000000",
                    "John", "Doe", false, false,
                    DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(2)));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "HotelId");
        }
    }
}
