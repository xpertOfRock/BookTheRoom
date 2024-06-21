using BookTheRoom.Core.Entities;

namespace BookTheRoom.Core.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<List<Order>> GetUserOrders(string userId);
        Task<List<Order>> GetActiveOrders();
        Task<List<Order>> GetExpiredOrders();
        
    }
}
