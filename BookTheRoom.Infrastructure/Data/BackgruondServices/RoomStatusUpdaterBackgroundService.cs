using BookTheRoom.Infrastructure.Data.Interfaces; 
using BookTheRoom.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BookTheRoom.Core.Enums;
using BookTheRoom.Application.UseCases.Queries.Order;
using MediatR;
using BookTheRoom.Application.UseCases.Queries.Room;
using BookTheRoom.Application.UseCases.Commands.Room;

namespace BookTheRoom.Infrastructure.Data.BackgruondServices
{
    public class RoomStatusUpdaterBackgroundService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RoomStatusUpdaterBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        private async Task UpdateRoomStatus()
        {
            using (var scope = _scopeFactory.CreateScope())
            {

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var activeOrders = await mediator.Send(new GetActiveOrdersQuery());

                var rooms = await mediator.Send(new GetAllRoomsQuery());

                foreach (var room in rooms)
                {
                    bool hasActiveOrder = activeOrders.Any(order => order.RoomId == room.Id);

                    if (hasActiveOrder)
                    {
                        var activeOrder = activeOrders.FirstOrDefault(order => order.RoomId == room.Id);
                        if (activeOrder != null && activeOrder.Status != OrderStatus.Closed && activeOrder.Status != OrderStatus.Completed)
                        {
                            room.IsFree = false;
                        }
                    }
                    else
                    {
                        List<Order> ordersForRoom = activeOrders.Where(order => order.RoomId == room.Id).ToList();

                        bool isRoomBooked = ordersForRoom.Any(order =>
                            DateTime.Now >= order.CheckIn && DateTime.Now <= order.CheckOut &&
                            (order.Status != OrderStatus.Closed && order.Status != OrderStatus.Completed));

                        if (!isRoomBooked)
                        {
                            room.IsFree = true;
                        }
                    }
                    await mediator.Send(new UpdateRoomCommand(room));
                }
            }
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        { 
            var _timer = new Timer(async entry =>
            {
                await UpdateRoomStatus();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
