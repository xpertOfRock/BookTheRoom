using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAll(GetOrdersRequest request, CancellationToken token = default);
        Task<Dictionary<Order, (Hotel, Room)>> GetAllUserOrders(string userId, GetOrdersRequest request, CancellationToken token = default);
        Task<List<Order>?> GetActiveOrders(CancellationToken token = default);
        Task<List<Order>?> GetExpiredOrders(CancellationToken token = default);
        //Task<Order> GetById(int id);
        Task<IResult> Add(Order order, CancellationToken token = default);
        Task Update(int id, UpdateOrderRequest request, CancellationToken token = default);

    }
}
