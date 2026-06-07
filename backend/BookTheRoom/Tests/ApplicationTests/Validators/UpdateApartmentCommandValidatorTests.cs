using Application.UseCases.Commands.Apartment;
using Application.UseCases.Validators.Apartment;
using Core.Contracts;
using Core.ValueObjects;

namespace Tests.ApplicationTests.Validators
{
    // Unit tests for UpdateApartmentCommandValidator.
    public class UpdateApartmentCommandValidatorTests
    {
        private readonly UpdateApartmentCommandValidator _validator = new();

        private static readonly Address ValidAddress =
            new Address("Ukraine", "Kyiv Oblast", "Kyiv", "Main St 1", "01001");

        private static UpdateApartmentCommand ValidCommand() =>
            new UpdateApartmentCommand(
                id: 1,
                userId: "user-001",
                request: new UpdateApartmentRequest(
                    Title: "Nice Flat",
                    Description: "A great place to stay",
                    Price: 50m,
                    Address: ValidAddress,
                    Images: new List<string> { "https://example.com/img.jpg" },
                    Telegram: "@owner",
                    Instagram: "@owner_ig"));

        [Fact]
        public void Validate_WithValidCommand_Passes()
        {
            var result = _validator.Validate(ValidCommand());
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("ab")]
        public void Validate_WithTooShortTitle_Fails(string title)
        {
            var cmd = new UpdateApartmentCommand(1, "u", new UpdateApartmentRequest(
                title, "desc", 50m, ValidAddress,
                new List<string> { "https://example.com/img.jpg" }, null, null));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Title"));
        }

        [Fact]
        public void Validate_WithZeroPrice_Fails()
        {
            var cmd = new UpdateApartmentCommand(1, "u", new UpdateApartmentRequest(
                "Valid Title", "desc", 0m, ValidAddress,
                new List<string> { "https://example.com/img.jpg" }, null, null));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Price"));
        }

        [Fact]
        public void Validate_WithNullAddress_Fails()
        {
            var cmd = new UpdateApartmentCommand(1, "u", new UpdateApartmentRequest(
                "Valid Title", "desc", 50m, null!,
                new List<string> { "https://example.com/img.jpg" }, null, null));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Address"));
        }

        [Fact]
        public void Validate_WithInvalidTelegramFormat_Fails()
        {
            var cmd = new UpdateApartmentCommand(1, "u", new UpdateApartmentRequest(
                "Valid Title", "desc", 50m, ValidAddress,
                new List<string> { "https://example.com/img.jpg" },
                Telegram: "no_at_sign", Instagram: null));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Telegram"));
        }

        [Fact]
        public void Validate_WithNullTelegramAndInstagram_Passes()
        {
            var cmd = new UpdateApartmentCommand(1, "u", new UpdateApartmentRequest(
                "Valid Title", "description here", 50m, ValidAddress,
                new List<string> { "https://example.com/img.jpg" },
                Telegram: null, Instagram: null));

            var result = _validator.Validate(cmd);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithMoreThan20Images_Fails()
        {
            var images = Enumerable.Range(1, 21)
                .Select(i => $"https://example.com/img{i}.jpg")
                .ToList();

            var cmd = new UpdateApartmentCommand(1, "u", new UpdateApartmentRequest(
                "Valid Title", "desc", 50m, ValidAddress, images, null, null));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Images"));
        }
    }
}
