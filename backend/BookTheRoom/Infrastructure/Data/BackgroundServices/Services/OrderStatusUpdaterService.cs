
namespace Infrastructure.Data.BackgroundServices.Services
{
    public class OrderStatusUpdaterService(IUnitOfWork unitOfWork) : IOrderStatusUpdaterService
    {
        public async Task UpdateOrderStatus()
        {
            var orders = await unitOfWork.Orders.GetAll(new GetOrdersRequest(null, null, null));

            orders = orders
                .Where(o => 
                    o.Status == OrderStatus.Active ||
                    o.Status == OrderStatus.Awaiting)
                .ToList();

            if (!orders.Any()) return;

            foreach (var order in orders)
            {

                if (order.Status != OrderStatus.Completed && order.CheckOut <= DateTime.UtcNow.Date)
                {
                    await unitOfWork.Orders.Update(order.Id, new UpdateOrderRequest(OrderStatus.Completed));
                }

                if (order.Status != OrderStatus.Active && order.CheckIn <= DateTime.UtcNow.Date && order.CheckOut > DateTime.UtcNow.Date)
                {
                    await unitOfWork.Orders.Update(order.Id, new UpdateOrderRequest(OrderStatus.Active));
                }
            }
        }
    }
}
