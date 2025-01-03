using Application.UseCases.Commands.Room;
using Application.UseCases.Queries.Order;
using Application.UseCases.Queries.Room;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Data.BackgroundServices
{
    public class RoomStatusUpdater : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RoomStatusUpdater(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        private async Task UpdateRoomStatus()
        {

            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var activeOrders = await mediator.Send(new GetActiveOrdersQuery());

            if (!activeOrders.Any()) return;

            var rooms = await mediator.Send(new GetRoomsQuery());

            foreach (var room in rooms)
            {
                bool hasActiveOrder = activeOrders.Any(order => order.HotelId == room.HotelId
                                                             && order.RoomNumber == room.Number);
                bool roomIsFree = room.IsFree;              

                if (hasActiveOrder)
                {
                    var activeOrder = activeOrders.FirstOrDefault(order => order.HotelId == room.HotelId
                                                                       && order.RoomNumber == room.Number);

                    if (activeOrder != null && activeOrder.Status == OrderStatus.Active)
                    {
                        roomIsFree = false;
                    }
                }
                else
                {
                    List<Order> ordersForRoom = activeOrders
                        .Where(order => order.HotelId == room.HotelId
                                     && order.RoomNumber == room.Number).ToList();

                    bool isRoomBooked = ordersForRoom.Any(order =>
                        DateTime.UtcNow >= order.CheckIn && DateTime.UtcNow <= order.CheckOut &&
                        (order.Status != OrderStatus.Closed && order.Status != OrderStatus.Completed));

                    if (!isRoomBooked)
                    {
                        roomIsFree = true;
                    }
                }

                var request = new UpdateRoomRequest(room.Name, room.Description, room.Price, room.Category, room.Images, roomIsFree);

                await mediator.Send(new UpdateRoomCommand(room.HotelId, room.Number, request));
            }
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var _timer = new Timer(async entry =>
            {
                await UpdateRoomStatus();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
