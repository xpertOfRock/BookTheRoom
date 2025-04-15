using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface which represents an abstraction for managing entities type of "Order" in database using Redis and Entity Framework with cache aside pattern.
    /// </summary>
    public interface IOrderRepository
    {
        Task<List<Order>> GetAll(GetOrdersRequest request);
        Task<List<Order>?> GetAllUserOrders(string userId, GetDataRequest request);
        Task<List<Order>?> GetActiveOrders();
        Task<List<Order>?> GetExpiredOrders();
        //Task<Order> GetById(int id);
        Task<IResult> Add(Order order);
        Task Update(int id, UpdateOrderRequest request);

    }
}
