using Core.Contracts;
using Core.Entities;
using Core.ValueObjects;
using Infrastructure.Data.Repositories;

namespace Tests.IntegrationTests
{
    /// <summary>
    /// Integration tests for <see cref="HotelRepository"/> against a live PostgreSQL
    /// database provided by Testcontainers.
    /// </summary>
    [Collection("Database")]
    public class HotelRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public HotelRepositoryIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        private static Hotel BuildHotel(string nameSuffix = "") => new Hotel
        {
            Name = $"Grand Hotel {nameSuffix}{Guid.NewGuid():N}",
            Description = "Integration test hotel",
            Rating = 4,
            HasPool = true,
            Address = new Address("Ukraine", "Lviv Oblast", "Lviv", "Svobody Ave 1", "79000"),
            Images = new List<string> { "https://example.com/img1.jpg" },
            Comments = new List<Comment>()
        };

        [Fact]
        public async Task Add_WithValidHotel_PersistsToDatabase()
        {
            await using var ctx = _fixture.CreateContext();
            var repo = new HotelRepository(ctx, _fixture.CreateNoOpCache(),
                                           _fixture.CreatePhotoServiceMock().Object);

            var hotel = BuildHotel();
            var result = await repo.Add(hotel, CancellationToken.None);
            await ctx.SaveChangesAsync();

            Assert.True(result.IsSuccess);
            Assert.True(hotel.Id > 0);

            await using var verify = _fixture.CreateContext();
            var saved = await verify.Hotels.FindAsync(hotel.Id);
            Assert.NotNull(saved);
            Assert.Equal(hotel.Name, saved!.Name);
        }

        [Fact]
        public async Task Add_DuplicateNameAndAddress_ReturnsFail()
        {
            await using var ctx1 = _fixture.CreateContext();
            var repo1 = new HotelRepository(ctx1, _fixture.CreateNoOpCache(),
                                            _fixture.CreatePhotoServiceMock().Object);

            var hotel = BuildHotel("Dup");
            await repo1.Add(hotel, CancellationToken.None);
            await ctx1.SaveChangesAsync();

            // Try to add the same hotel again with the same name+address.
            await using var ctx2 = _fixture.CreateContext();
            var repo2 = new HotelRepository(ctx2, _fixture.CreateNoOpCache(),
                                            _fixture.CreatePhotoServiceMock().Object);

            var duplicate = new Hotel
            {
                Name = hotel.Name,
                Description = "duplicate",
                Rating = 3,
                HasPool = false,
                Address = hotel.Address,
                Images = new List<string>(),
                Comments = new List<Comment>()
            };

            var result = await repo2.Add(duplicate, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetById_ExistingHotel_ReturnsCorrectEntity()
        {
            await using var seedCtx = _fixture.CreateContext();
            var hotel = BuildHotel();
            seedCtx.Hotels.Add(hotel);
            await seedCtx.SaveChangesAsync();

            await using var ctx = _fixture.CreateContext();
            var repo = new HotelRepository(ctx, _fixture.CreateNoOpCache(),
                                           _fixture.CreatePhotoServiceMock().Object);

            var fetched = await repo.GetById(hotel.Id, CancellationToken.None);

            Assert.NotNull(fetched);
            Assert.Equal(hotel.Name, fetched!.Name);
            Assert.Equal("Lviv", fetched.Address.City);
        }

        [Fact]
        public async Task Delete_ExistingHotel_RemovesFromDatabase()
        {
            await using var seedCtx = _fixture.CreateContext();
            var hotel = BuildHotel();
            seedCtx.Hotels.Add(hotel);
            await seedCtx.SaveChangesAsync();

            await using var ctx = _fixture.CreateContext();
            var repo = new HotelRepository(ctx, _fixture.CreateNoOpCache(),
                                           _fixture.CreatePhotoServiceMock().Object);

            var result = await repo.Delete(hotel.Id, CancellationToken.None);

            Assert.True(result.IsSuccess);

            await using var verify = _fixture.CreateContext();
            var deleted = await verify.Hotels.FindAsync(hotel.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Update_ExistingHotel_ChangesNameAndRating()
        {
            await using var seedCtx = _fixture.CreateContext();
            var hotel = BuildHotel();
            seedCtx.Hotels.Add(hotel);
            await seedCtx.SaveChangesAsync();

            await using var ctx = _fixture.CreateContext();
            var repo = new HotelRepository(ctx, _fixture.CreateNoOpCache(),
                                           _fixture.CreatePhotoServiceMock().Object);

            var request = new UpdateHotelRequest(
                Name: "Updated Name",
                Description: "Updated Desc",
                Rating: 5,
                HasPool: false,
                Address: hotel.Address,
                Images: null);

            var result = await repo.Update(hotel.Id, request, CancellationToken.None);

            Assert.True(result.IsSuccess);

            await using var verify = _fixture.CreateContext();
            var updated = await verify.Hotels.FindAsync(hotel.Id);
            Assert.Equal("Updated Name", updated!.Name);
            Assert.Equal(5, updated.Rating);
        }

        [Fact]
        public async Task GetAll_WithCountryFilter_ReturnsOnlyMatchingHotels()
        {
            await using var seedCtx = _fixture.CreateContext();

            var ukraineHotel = BuildHotel("UA");
            var polandHotel = new Hotel
            {
                Name = $"Poland Hotel {Guid.NewGuid():N}",
                Description = "desc",
                Rating = 3,
                HasPool = false,
                Address = new Address("Poland", "Mazovia", "Warsaw", "Main St 1", "00-001"),
                Images = new List<string>(),
                Comments = new List<Comment>()
            };

            seedCtx.Hotels.AddRange(ukraineHotel, polandHotel);
            await seedCtx.SaveChangesAsync();

            await using var ctx = _fixture.CreateContext();
            var repo = new HotelRepository(ctx, _fixture.CreateNoOpCache(),
                                           _fixture.CreatePhotoServiceMock().Object);

            var request = new GetHotelsRequest(
                Search: null, SortItem: null, SortOrder: null,
                Countries: "Ukraine", Ratings: null,
                Services: null, UserScore: null);

            var hotels = await repo.GetAll(request, CancellationToken.None);

            Assert.All(hotels, h => Assert.Equal("Ukraine", h.Address.Country));
        }
    }
}
