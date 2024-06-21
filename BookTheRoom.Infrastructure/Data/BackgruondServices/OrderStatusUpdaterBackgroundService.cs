using BookTheRoom.Application.UseCases.Commands.Order;
using BookTheRoom.Application.UseCases.Queries.Order;
using BookTheRoom.Core.Enums;
using BookTheRoom.Infrastructure.Data.Interfaces;
using MediatR;
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
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var expiredOrders = await mediator.Send(new GetExpiredOrdersQuery());

                foreach (var order in expiredOrders)
                {
                    if (order.Status == OrderStatus.Active)
                    {
                        order.Status = OrderStatus.Completed;
                        await mediator.Send(new UpdateOrderCommand(order));
                    }
                }
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
