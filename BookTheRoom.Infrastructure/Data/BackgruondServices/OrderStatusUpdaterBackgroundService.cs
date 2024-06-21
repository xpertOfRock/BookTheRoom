using BookTheRoom.Application.Interfaces;
using BookTheRoom.Core.Entities;
using BookTheRoom.Core.Enums;
using BookTheRoom.Infrastructure.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookTheRoom.Infrastructure.Data.BackgruondServices
{
    public class OrderStatusUpdaterBackgroundService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderStatusUpdaterBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        private async Task UpdateOrderStatus()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var expiredOrders = await unitOfWork.Orders.GetExpiredOrders();

                foreach (var order in expiredOrders)
                {
                    if (order.Status == OrderStatus.Active)
                    {
                        order.Status = OrderStatus.Completed;
                        unitOfWork.Orders.Update(order);

                    }
                }
                await unitOfWork.SaveChangesAsync();
            }
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var _timer = new Timer(async entry =>
            {
                await UpdateOrderStatus();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
