using BookTheRoom.Domain.Entities;

namespace BookTheRoom.Application.Interfaces
{
    public interface IOrderRepository
    {
        //Task<Order> GetOrderByIdAsync(int id);
        Task<List<Order>> GetAllOrders();
        Task Add(Order order);
        //Task Delete(Order order);
        //Task Update(Order order);
    }
}
