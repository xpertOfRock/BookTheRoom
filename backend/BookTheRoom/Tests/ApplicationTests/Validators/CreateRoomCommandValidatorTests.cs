using Application.UseCases.Commands.Room;
using Application.UseCases.Validators.Room;
using Core.Contracts;
using Core.Enums;

namespace Tests.ApplicationTests.Validators
{
    // Unit tests for CreateRoomCommandValidator.
    public class CreateRoomCommandValidatorTests
    {
        private readonly CreateRoomCommandValidator _validator = new();

        private static CreateRoomCommand ValidCommand() =>
            new CreateRoomCommand(hotelId: 1, new CreateRoomRequest(
                Name: "Standard",
                Description: "A nice room",
                Number: 101,
                Price: 100m,
                Category: RoomCategory.OneBedApartments,
                Images: new List<string> { "https://example.com/img.jpg" }));

        [Fact]
        public void Validate_WithValidCommand_Passes()
        {
            var result = _validator.Validate(ValidCommand());
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_WithEmptyName_Fails(string? name)
        {
            var cmd = new CreateRoomCommand(1, new CreateRoomRequest(
                name!, "desc", 101, 100m, RoomCategory.OneBedApartments,
                new List<string> { "https://example.com/img.jpg" }));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        }

        [Fact]
        public void Validate_WithNameExceeding100Chars_Fails()
        {
            var cmd = new CreateRoomCommand(1, new CreateRoomRequest(
                new string('x', 101), "desc", 101, 100m, RoomCategory.OneBedApartments,
                new List<string> { "https://example.com/img.jpg" }));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        }

        [Fact]
        public void Validate_WithZeroNumber_Fails()
        {
            var cmd = new CreateRoomCommand(1, new CreateRoomRequest(
                "Standard", "desc", 0, 100m, RoomCategory.OneBedApartments,
                new List<string> { "https://example.com/img.jpg" }));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Number");
        }

        [Fact]
        public void Validate_WithNegativePrice_Fails()
        {
            var cmd = new CreateRoomCommand(1, new CreateRoomRequest(
                "Standard", "desc", 101, -1m, RoomCategory.OneBedApartments,
                new List<string> { "https://example.com/img.jpg" }));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Price");
        }

        [Fact]
        public void Validate_WithZeroHotelId_Fails()
        {
            var cmd = new CreateRoomCommand(0, new CreateRoomRequest(
                "Standard", "desc", 101, 100m, RoomCategory.OneBedApartments,
                new List<string> { "https://example.com/img.jpg" }));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "HotelId");
        }

        [Fact]
        public void Validate_WithMoreThan20Images_Fails()
        {
            var images = Enumerable.Range(1, 21)
                .Select(i => $"https://example.com/img{i}.jpg")
                .ToList();

            var cmd = new CreateRoomCommand(1, new CreateRoomRequest(
                "Standard", "desc", 101, 100m, RoomCategory.OneBedApartments, images));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Images");
        }

        [Fact]
        public void Validate_WithInvalidImageUrl_Fails()
        {
            var cmd = new CreateRoomCommand(1, new CreateRoomRequest(
                "Standard", "desc", 101, 100m, RoomCategory.OneBedApartments,
                new List<string> { "not-a-url" }));

            var result = _validator.Validate(cmd);
            Assert.False(result.IsValid);
        }
    }
}
