using Core.Contracts;
using Core.Entities;
using Core.Enums;
using Core.ValueObjects;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests
{
    /// <summary>
    /// Integration tests for <see cref="OrderRepository"/> against a real PostgreSQL
    /// database spun up via Testcontainers.
    /// These tests verify the overlap check and the Serializable concurrency guarantee.
    /// </summary>
    [Collection("Database")]
    public class OrderRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public OrderRepositoryIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        // Seeds a hotel and a room so that order tests have valid foreign keys.
        private async Task<(Hotel hotel, Room room)> SeedHotelAndRoomAsync()
        {
            await using var ctx = _fixture.CreateContext();

            var hotel = new Hotel
            {
                Name = $"Test Hotel {Guid.NewGuid():N}",
                Description = "Integration test hotel",
                Rating = 4,
                HasPool = false,
                Address = new Address("Ukraine", "Lviv Oblast", "Lviv", "Test St 1", "79000"),
                Images = new List<string>(),
                Comments = new List<Comment>()
            };

            ctx.Hotels.Add(hotel);
            await ctx.SaveChangesAsync();

            var room = new Room
            {
                HotelId = hotel.Id,
                Number = 101,
                Name = "Standard",
                Description = "A standard room",
                Price = 100m,
                Category = RoomCategory.OneBedApartments,
                Images = new List<string>()
            };

            ctx.Rooms.Add(room);
            await ctx.SaveChangesAsync();

            return (hotel, room);
        }

        private static Order BuildOrder(int hotelId, int roomNumber,
            DateTime checkIn, DateTime checkOut) => new Order
        {
            HotelId = hotelId,
            RoomNumber = roomNumber,
            Email = "test@example.com",
            Phone = "+380000000000",
            FirstName = "John",
            LastName = "Doe",
            CheckIn = checkIn.ToUniversalTime(),
            CheckOut = checkOut.ToUniversalTime(),
            OverallPrice = 200m,
            Status = OrderStatus.Awaiting,
            CreatedAt = DateTime.UtcNow
        };

        [Fact]
        public async Task Add_WhenNoConflict_ReturnsSuccessAndPersistsOrder()
        {
            var (hotel, room) = await SeedHotelAndRoomAsync();
            await using var ctx = _fixture.CreateContext();
            var repo = new OrderRepository(ctx, _fixture.CreateNoOpCache());

            var checkIn = DateTime.UtcNow.Date.AddDays(10);
            var order = BuildOrder(hotel.Id, room.Number, checkIn, checkIn.AddDays(3));

            var result = await repo.Add(order, CancellationToken.None);
            await ctx.SaveChangesAsync();

            Assert.True(result.IsSuccess);
            var saved = await ctx.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Add_WhenOverlappingBookingExists_ReturnsFail()
        {
            var (hotel, room) = await SeedHotelAndRoomAsync();

            var checkIn = DateTime.UtcNow.Date.AddDays(20);
            var checkOut = checkIn.AddDays(5);

            // Insert first booking directly so it is committed.
            await using (var seedCtx = _fixture.CreateContext())
            {
                var firstOrder = BuildOrder(hotel.Id, room.Number, checkIn, checkOut);
                seedCtx.Orders.Add(firstOrder);
                await seedCtx.SaveChangesAsync();
            }

            // Second attempt overlaps with the first.
            await using var ctx = _fixture.CreateContext();
            var repo = new OrderRepository(ctx, _fixture.CreateNoOpCache());

            var overlapping = BuildOrder(hotel.Id, room.Number,
                checkIn.AddDays(2),  // starts inside the existing booking
                checkOut.AddDays(2));

            var result = await repo.Add(overlapping, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorStatuses.ConflictError, ((Core.TasksResults.Fail)result).Status);
        }

        [Fact]
        public async Task Add_WhenBookingsAreAdjacentButNonOverlapping_BothSucceed()
        {
            var (hotel, room) = await SeedHotelAndRoomAsync();

            var firstCheckIn = DateTime.UtcNow.Date.AddDays(30);
            var firstCheckOut = firstCheckIn.AddDays(3);

            await using (var seedCtx = _fixture.CreateContext())
            {
                seedCtx.Orders.Add(BuildOrder(hotel.Id, room.Number, firstCheckIn, firstCheckOut));
                await seedCtx.SaveChangesAsync();
            }

            // Second booking starts exactly on the day the first ends — no overlap.
            await using var ctx = _fixture.CreateContext();
            var repo = new OrderRepository(ctx, _fixture.CreateNoOpCache());

            var adjacent = BuildOrder(hotel.Id, room.Number, firstCheckOut, firstCheckOut.AddDays(3));
            var result = await repo.Add(adjacent, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Add_CompletedOrdersAreIgnoredInConflictCheck()
        {
            var (hotel, room) = await SeedHotelAndRoomAsync();

            var checkIn = DateTime.UtcNow.Date.AddDays(40);
            var checkOut = checkIn.AddDays(3);

            // Insert a COMPLETED order for the same dates.
            await using (var seedCtx = _fixture.CreateContext())
            {
                var completed = BuildOrder(hotel.Id, room.Number, checkIn, checkOut);
                completed.Status = OrderStatus.Completed;
                seedCtx.Orders.Add(completed);
                await seedCtx.SaveChangesAsync();
            }

            // A new booking for the same dates should be allowed.
            await using var ctx = _fixture.CreateContext();
            var repo = new OrderRepository(ctx, _fixture.CreateNoOpCache());

            var newOrder = BuildOrder(hotel.Id, room.Number, checkIn, checkOut);
            var result = await repo.Add(newOrder, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Add_ConcurrentBookingsForSameRoom_OnlyOneSucceeds()
        {
            // This test validates the Serializable isolation + overlap check combination:
            // two concurrent transactions racing to book the same room must produce
            // exactly one committed order and one failure/rollback.
            var (hotel, room) = await SeedHotelAndRoomAsync();

            var checkIn = DateTime.UtcNow.Date.AddDays(60);
            var checkOut = checkIn.AddDays(3);

            async Task<bool> TryBookAsync()
            {
                await using var ctx = _fixture.CreateContext();
                await ctx.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

                try
                {
                    var repo = new OrderRepository(ctx, _fixture.CreateNoOpCache());
                    var order = BuildOrder(hotel.Id, room.Number, checkIn, checkOut);

                    var result = await repo.Add(order, CancellationToken.None);

                    if (!result.IsSuccess)
                    {
                        await ctx.Database.CurrentTransaction!.RollbackAsync();
                        return false;
                    }

                    await ctx.SaveChangesAsync();
                    await ctx.Database.CurrentTransaction!.CommitAsync();
                    return true;
                }
                catch
                {
                    // Serialization failure (Npgsql error 40001) or any other exception
                    // counts as a booking failure — the transaction is rolled back.
                    try { await ctx.Database.CurrentTransaction?.RollbackAsync()!; }
                    catch { /* already rolled back */ }

                    return false;
                }
            }

            var results = await Task.WhenAll(TryBookAsync(), TryBookAsync());

            // Exactly one booking must have committed.
            Assert.Equal(1, results.Count(r => r));

            await using var verifyCtx = _fixture.CreateContext();
            var orderCount = await verifyCtx.Orders
                .CountAsync(o => o.HotelId == hotel.Id &&
                                 o.RoomNumber == room.Number &&
                                 o.CheckIn == checkIn.ToUniversalTime());

            Assert.Equal(1, orderCount);
        }
    }
}
