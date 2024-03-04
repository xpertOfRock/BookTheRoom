using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order> , IOrderRepository
    {       
        public OrderRepository(ApplicationDbContext context) : base(context) {
        }
       
    }
}
