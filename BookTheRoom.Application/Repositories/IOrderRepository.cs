using BookTheRoom.Domain.Entities;
using BookTheRoom.Domain.Enums;

namespace BookTheRoom.Application.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<List<Order>> GetUserOrders(string userId);
        Task<List<Order>> GetActiveOrders();
        Task<List<Order>> GetExpiredOrders();
        
    }
}
