using Application.UseCases.Commands.Order;
using Application.UseCases.Queries.Order;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.BackgroundServices
{
    public class OrderStatusUpdater(IServiceScopeFactory scopeFactory, ILogger<OrderStatusUpdater> logger) : IHostedService
    {
        private async Task UpdateOrderStatus()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var orders = await mediator.Send(new GetAllOrdersQuery(new GetOrdersRequest(null, null, null)));

                if (orders.Any())
                {
                    foreach (var order in orders)
                    {

                        if (order.Status == OrderStatus.Active && order.CheckOut <= DateTime.UtcNow.Date)
                        {
                            await mediator.Send(new UpdateOrderCommand(order.Id, new UpdateOrderRequest(OrderStatus.Completed)));
                        }

                        if (order.Status == OrderStatus.Awaiting && order.CheckIn <= DateTime.UtcNow.Date && order.CheckOut > DateTime.UtcNow.Date)
                        {
                            await mediator.Send(new UpdateOrderCommand(order.Id, new UpdateOrderRequest(OrderStatus.Active)));
                        }
                    }
                }
            }


        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] OrderStatusUpdater background service has been started.");
            var _timer = new Timer(async entry =>
            {
                await UpdateOrderStatus();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("[STOP] OrderStatusUpdater background service has been stoped.");
            return Task.CompletedTask;
        }
    }
}
