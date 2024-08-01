using Application.UseCases.Commands.Order;
using Application.UseCases.Queries.Order;
using Core.Contracts;
using Core.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Data.BackgroundServices
{
    public class OrderStatusUpdater : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderStatusUpdater(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        private async Task UpdateOrderStatus()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var expiredOrders = await mediator.Send(new GetExpiredOrdersQuery());

                if(expiredOrders.Any())
                {
                    foreach (var order in expiredOrders)
                    {
                        var request = new UpdateOrderRequest(
                            order.MinibarIncluded,
                            order.MealsIncluded, order.Status
                            );

                        if (order.Status == OrderStatus.Active)
                        {
                            order.Status = OrderStatus.Completed;
                            await mediator.Send(new UpdateOrderCommand(order.Id, request));
                        }
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
