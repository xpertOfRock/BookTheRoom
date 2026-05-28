using Application.UseCases.Commands.Hotel;
using Application.UseCases.Validators.Hotel;
using Core.Contracts;
using Core.ValueObjects;
using FluentValidation.Results;

namespace Tests.ApplicationTests.Validators
{
    public class CreateHotelCommandValidatorTests
    {
        private readonly CreateHotelCommandValidator _validator = new();

        private static Address ValidAddress() =>
            new Address("Ukraine", "Lviv Oblast", "Lviv", "Svobody Ave 1", "79000");

        private static CreateHotelCommand BuildCommand(
            string name = "Grand Hotel",
            string description = "A pleasant place to stay.",
            int rating = 4,
            List<string>? images = null)
        {
            var request = new CreateHotelRequest(
                Name: name,
                Description: description,
                Rating: rating,
                Pool: true,
                Address: ValidAddress(),
                Images: images ?? new List<string> { "https://example.com/image1.jpg" });

            return new CreateHotelCommand(request);
        }

        [Fact]
        public void Validate_WithValidCommand_PassesValidation()
        {
            var command = BuildCommand();

            ValidationResult result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(-1)]
        public void Validate_WithRatingOutsideAllowedRange_Fails(int invalidRating)
        {
            var command = BuildCommand(rating: invalidRating);

            ValidationResult result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Rating"));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void Validate_WithRatingInsideAllowedRange_Passes(int validRating)
        {
            var command = BuildCommand(rating: validRating);

            ValidationResult result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithEmptyName_Fails()
        {
            var command = BuildCommand(name: "");

            ValidationResult result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("Name"));
        }

        [Fact]
        public void Validate_WithMoreThanTwentyImages_Fails()
        {
            var images = Enumerable
                .Range(1, 21)
                .Select(i => $"https://example.com/image{i}.jpg")
                .ToList();

            var command = BuildCommand(images: images);

            ValidationResult result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WithMalformedImageUrl_Fails()
        {
            var command = BuildCommand(images: new List<string> { "not-a-valid-url" });

            ValidationResult result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }
    }
}
