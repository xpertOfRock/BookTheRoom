using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.BackgroundServices
{
    public class OrderStatusUpdaterBackgroundService(IServiceScopeFactory scopeFactory, ILogger<OrderStatusUpdaterBackgroundService> logger) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("[START OrderStatusUpdater] background service has been started.");
            var _timer = new Timer(async entry =>
            {
                using var scope = scopeFactory.CreateScope();

                var updater = scope.ServiceProvider.GetRequiredService<IOrderStatusUpdaterService>();

                await updater.UpdateOrderStatus();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("[STOP OrderStatusUpdater] background service has been stoped.");
            return Task.CompletedTask;
        }
    }
}
