using Core.Contracts;
using Core.Entities;
using Core.ValueObjects;
using Infrastructure.Data.Repositories;

namespace Tests.IntegrationTests
{
    /// <summary>
    /// Integration tests for <see cref="ApartmentRepository"/> against a live PostgreSQL
    /// database provided by Testcontainers.
    /// </summary>
    [Collection("Database")]
    public class ApartmentRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        private const string OwnerId = "user-owner-001";
        private const string OtherUserId = "user-other-002";

        public ApartmentRepositoryIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        private static Apartment BuildApartment(string ownerId = OwnerId) => new Apartment
        {
            OwnerId = ownerId,
            OwnerName = "Test Owner",
            Title = $"Cozy Flat {Guid.NewGuid():N}",
            Description = "Integration test apartment",
            PriceForNight = 50m,
            Email = "owner@example.com",
            PhoneNumber = "+380000000000",
            Address = new Address("Ukraine", "Kyiv Oblast", "Kyiv",
                                  $"Test St {Guid.NewGuid():N}", "01001"),
            Images = new List<string>(),
            Comments = new List<Comment>()
        };

        [Fact]
        public async Task Add_WithValidApartment_PersistsToDatabase()
        {
            await using var ctx = _fixture.CreateContext();
            var repo = new ApartmentRepository(ctx, _fixture.CreateNoOpCache(),
                                               _fixture.CreatePhotoServiceMock().Object);

            var apt = BuildApartment();
            var result = await repo.Add(apt, CancellationToken.None);
            await ctx.SaveChangesAsync();

            Assert.True(result.IsSuccess);
            Assert.True(apt.Id > 0);

            await using var verify = _fixture.CreateContext();
            var saved = await verify.Apartments.FindAsync(apt.Id);
            Assert.NotNull(saved);
            Assert.Equal(apt.Title, saved!.Title);
        }

        [Fact]
        public async Task Add_DuplicateAddress_ReturnsFail()
        {
            var apt = BuildApartment();

            await using (var seedCtx = _fixture.CreateContext())
            {
                seedCtx.Apartments.Add(apt);
                await seedCtx.SaveChangesAsync();
            }

            // Second apartment with the same address.
            await using var ctx = _fixture.CreateContext();
            var repo = new ApartmentRepository(ctx, _fixture.CreateNoOpCache(),
                                               _fixture.CreatePhotoServiceMock().Object);

            var duplicate = new Apartment
            {
                OwnerId = OwnerId,
                OwnerName = "Other Owner",
                Title = "Another Title",
                Description = "desc",
                PriceForNight = 80m,
                Email = "other@example.com",
                PhoneNumber = "+380000000001",
                Address = apt.Address,
                Images = new List<string>(),
                Comments = new List<Comment>()
            };

            var result = await repo.Add(duplicate, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetById_ExistingApartment_ReturnsCorrectEntity()
        {
            var apt = BuildApartment();
            await using (var seedCtx = _fixture.CreateContext())
            {
                seedCtx.Apartments.Add(apt);
                await seedCtx.SaveChangesAsync();
            }

            await using var ctx = _fixture.CreateContext();
            var repo = new ApartmentRepository(ctx, _fixture.CreateNoOpCache(),
                                               _fixture.CreatePhotoServiceMock().Object);

            var fetched = await repo.GetById(apt.Id, CancellationToken.None);

            Assert.NotNull(fetched);
            Assert.Equal(apt.Title, fetched.Title);
            Assert.Equal("Kyiv", fetched.Address.City);
        }

        [Fact]
        public async Task Delete_ByOwner_RemovesFromDatabase()
        {
            var apt = BuildApartment();
            await using (var seedCtx = _fixture.CreateContext())
            {
                seedCtx.Apartments.Add(apt);
                await seedCtx.SaveChangesAsync();
            }

            await using var ctx = _fixture.CreateContext();
            var repo = new ApartmentRepository(ctx, _fixture.CreateNoOpCache(),
                                               _fixture.CreatePhotoServiceMock().Object);

            var result = await repo.Delete(apt.Id, OwnerId, CancellationToken.None);

            Assert.True(result.IsSuccess);

            await using var verify = _fixture.CreateContext();
            var deleted = await verify.Apartments.FindAsync(apt.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Delete_ByWrongUser_ReturnsFail_AndRecordStillExists()
        {
            var apt = BuildApartment();
            await using (var seedCtx = _fixture.CreateContext())
            {
                seedCtx.Apartments.Add(apt);
                await seedCtx.SaveChangesAsync();
            }

            await using var ctx = _fixture.CreateContext();
            var repo = new ApartmentRepository(ctx, _fixture.CreateNoOpCache(),
                                               _fixture.CreatePhotoServiceMock().Object);

            var result = await repo.Delete(apt.Id, OtherUserId, CancellationToken.None);

            Assert.False(result.IsSuccess);

            // Record must still be in the database.
            await using var verify = _fixture.CreateContext();
            var still = await verify.Apartments.FindAsync(apt.Id);
            Assert.NotNull(still);
        }

        [Fact]
        public async Task Delete_NonExistentApartment_ReturnsFail()
        {
            await using var ctx = _fixture.CreateContext();
            var repo = new ApartmentRepository(ctx, _fixture.CreateNoOpCache(),
                                               _fixture.CreatePhotoServiceMock().Object);

            var result = await repo.Delete(int.MaxValue, OwnerId, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Update_ByOwner_ChangesTitle()
        {
            var apt = BuildApartment();
            await using (var seedCtx = _fixture.CreateContext())
            {
                seedCtx.Apartments.Add(apt);
                await seedCtx.SaveChangesAsync();
            }

            await using var ctx = _fixture.CreateContext();
            var repo = new ApartmentRepository(ctx, _fixture.CreateNoOpCache(),
                                               _fixture.CreatePhotoServiceMock().Object);

            var request = new UpdateApartmentRequest(
                Title: "Updated Flat",
                Description: "new desc",
                Price: 75m,
                Address: apt.Address,
                Images: null,
                Telegram: null,
                Instagram: null);

            var result = await repo.Update(apt.Id, OwnerId, request, CancellationToken.None);

            Assert.True(result.IsSuccess);

            await using var verify = _fixture.CreateContext();
            var updated = await verify.Apartments.FindAsync(apt.Id);
            Assert.Equal("Updated Flat", updated!.Title);
            Assert.Equal(75m, updated.PriceForNight);
        }

        [Fact]
        public async Task Update_ByWrongUser_ReturnsFail()
        {
            var apt = BuildApartment();
            await using (var seedCtx = _fixture.CreateContext())
            {
                seedCtx.Apartments.Add(apt);
                await seedCtx.SaveChangesAsync();
            }

            await using var ctx = _fixture.CreateContext();
            var repo = new ApartmentRepository(ctx, _fixture.CreateNoOpCache(),
                                               _fixture.CreatePhotoServiceMock().Object);

            var request = new UpdateApartmentRequest(
                Title: "Hacked Title",
                Description: "desc",
                Price: 999m,
                Address: apt.Address,
                Images: null,
                Telegram: null,
                Instagram: null);

            var result = await repo.Update(apt.Id, OtherUserId, request, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetAll_WithCountriesFilter_ReturnsOnlyMatchingApartments()
        {
            await using var seedCtx = _fixture.CreateContext();

            var uaApt = BuildApartment();

            var plApt = new Apartment
            {
                OwnerId = OwnerId,
                OwnerName = "PL Owner",
                Title = $"Warsaw Flat {Guid.NewGuid():N}",
                Description = "desc",
                PriceForNight = 60m,
                Email = "pl@example.com",
                PhoneNumber = "+48000000000",
                Address = new Address("Poland", "Mazovia", "Warsaw",
                                      $"Main St {Guid.NewGuid():N}", "00-001"),
                Images = new List<string>(),
                Comments = new List<Comment>()
            };

            seedCtx.Apartments.AddRange(uaApt, plApt);
            await seedCtx.SaveChangesAsync();

            await using var ctx = _fixture.CreateContext();
            var repo = new ApartmentRepository(ctx, _fixture.CreateNoOpCache(),
                                               _fixture.CreatePhotoServiceMock().Object);

            var request = new GetApartmentsRequest(
                Search: null, SortItem: null, SortOrder: null,
                Countries: "Ukraine",
                MinPrice: null, MaxPrice: null);

            var apts = await repo.GetAll(request, CancellationToken.None);

            Assert.All(apts, a => Assert.Equal("Ukraine", a.Address.Country));
        }
    }
}
