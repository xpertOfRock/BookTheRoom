using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAll(GetOrdersRequest request);
        Task<List<Order>> GetAllUserOrders(string userId, GetUserOrdersRequest request);
        Task<List<Order>> GetActiveOrders();
        Task<List<Order>> GetExpiredOrders();
        //Task<Order> GetById(int id);
        Task Add(Order order);
        Task Update(int id, UpdateOrderRequest request);

    }
}
