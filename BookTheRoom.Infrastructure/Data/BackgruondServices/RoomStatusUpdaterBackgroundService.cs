using BookTheRoom.Infrastructure.Data.Interfaces; 
using BookTheRoom.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BookTheRoom.Domain.Enums;

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
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var activeOrders = await unitOfWork.Orders.GetActiveOrders();
                var rooms = await unitOfWork.Rooms.GetAll();

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
                    await unitOfWork.Rooms.Update(room);
                }
                await unitOfWork.SaveChangesAsync();
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
