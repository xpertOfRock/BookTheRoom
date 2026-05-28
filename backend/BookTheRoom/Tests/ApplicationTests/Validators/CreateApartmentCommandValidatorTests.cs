using Application.UseCases.Commands.Apartment;
using Application.UseCases.Validators.Apartment;
using Core.Contracts;
using Core.ValueObjects;
using FluentValidation.Results;

namespace Tests.ApplicationTests.Validators
{
    public class CreateApartmentCommandValidatorTests
    {
        private readonly CreateApartmentCommandValidator _validator = new();

        private static Address ValidAddress() =>
            new Address("Ukraine", "Lviv Oblast", "Lviv", "Svobody Ave 1", "79000");

        private static CreateApartmentCommand BuildCommand(
            string ownerId = "owner-1",
            string ownerName = "John Doe",
            string email = "john@example.com",
            string phoneNumber = "+380000000000",
            decimal price = 100m,
            string? telegram = "@john",
            string? instagram = null)
        {
            var request = new CreateApartmentRequest(
                Title: "Cozy apartment",
                Description: "A cozy apartment in the city center.",
                Price: price,
                Address: ValidAddress(),
                Images: new List<string> { "https://example.com/image1.jpg" },
                Telegram: telegram,
                Instagram: instagram);

            return new CreateApartmentCommand(ownerId, ownerName, email, phoneNumber, request);
        }

        [Fact]
        public void Validate_WithValidCommand_Passes()
        {
            var command = BuildCommand();

            ValidationResult result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void Validate_WithNonPositivePrice_Fails(decimal invalidPrice)
        {
            var command = BuildCommand(price: invalidPrice);

            ValidationResult result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Price"));
        }

        [Fact]
        public void Validate_WithInvalidEmail_Fails()
        {
            var command = BuildCommand(email: "not-an-email");

            ValidationResult result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Email"));
        }

        [Fact]
        public void Validate_WithTelegramWithoutAtSign_Fails()
        {
            var command = BuildCommand(telegram: "john");

            ValidationResult result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WithEmptyTelegram_Passes()
        {
            // Пустой/null username допустим — правило срабатывает только при наличии значения.
            var command = BuildCommand(telegram: "");

            ValidationResult result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithTooShortOwnerName_Fails()
        {
            var command = BuildCommand(ownerName: "Jo");

            ValidationResult result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("OwnerName"));
        }
    }
}
