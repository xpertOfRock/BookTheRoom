using Application.UseCases.Queries.Room;
using Application.UseCases.Validators.Room;
using Core.Contracts;

namespace Tests.ApplicationTests.Validators
{
    // Unit tests for GetHotelRoomsQueryValidator.
    // CheckIn must be at least tomorrow (UTC); CheckOut must be CheckIn + 1 day minimum.
    public class GetHotelRoomsQueryValidatorTests
    {
        private readonly GetHotelRoomsQueryValidator _validator = new();

        private static GetHotelRoomsQuery BuildQuery(DateTime? checkIn = null, DateTime? checkOut = null,
                                                      decimal? minPrice = null, decimal? maxPrice = null)
        {
            var ci = checkIn ?? DateTime.UtcNow.Date.AddDays(1);
            var co = checkOut ?? ci.AddDays(2);

            return new GetHotelRoomsQuery(hotelId: 1, new GetRoomsRequest(
                Search: null, SortItem: null, SortOrder: null, Categories: null,
                MinPrice: minPrice ?? 0m,
                MaxPrice: maxPrice ?? 500m,
                CheckIn: ci,
                CheckOut: co));
        }

        [Fact]
        public void Validate_WithValidFutureDates_Passes()
        {
            var query = BuildQuery();
            var result = _validator.Validate(query);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithCheckInToday_Fails()
        {
            var query = BuildQuery(checkIn: DateTime.UtcNow.Date,
                                   checkOut: DateTime.UtcNow.Date.AddDays(2));

            var result = _validator.Validate(query);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("CheckIn"));
        }

        [Fact]
        public void Validate_WithCheckInInThePast_Fails()
        {
            var query = BuildQuery(checkIn: DateTime.UtcNow.Date.AddDays(-1),
                                   checkOut: DateTime.UtcNow.Date.AddDays(1));

            var result = _validator.Validate(query);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WithCheckOutEqualsCheckIn_Fails()
        {
            var ci = DateTime.UtcNow.Date.AddDays(2);
            // CheckOut same day as CheckIn — must be at least CheckIn + 1 day.
            var query = BuildQuery(checkIn: ci, checkOut: ci);

            var result = _validator.Validate(query);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("CheckOut"));
        }

        [Fact]
        public void Validate_WithCheckOutBeforeCheckIn_Fails()
        {
            var ci = DateTime.UtcNow.Date.AddDays(3);
            var query = BuildQuery(checkIn: ci, checkOut: ci.AddDays(-1));

            var result = _validator.Validate(query);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WithNegativeMinPrice_Fails()
        {
            var query = BuildQuery(minPrice: -1m, maxPrice: 100m);

            var result = _validator.Validate(query);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("MinPrice"));
        }

        [Fact]
        public void Validate_WithMaxPriceLessThanMinPrice_Fails()
        {
            var query = BuildQuery(minPrice: 200m, maxPrice: 100m);

            var result = _validator.Validate(query);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName.Contains("MaxPrice"));
        }
    }
}
