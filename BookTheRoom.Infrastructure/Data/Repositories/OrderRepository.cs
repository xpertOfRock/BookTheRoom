using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using BookTheRoom.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order> , IOrderRepository
    {
        private readonly IMemoryCache _memoryCache;
        public OrderRepository(ApplicationDbContext context, IMemoryCache memoryCache) : base(context)
        {
            _memoryCache = memoryCache;
        }

        public override async Task<List<Order>> GetAll()
        {
            return await ApplicationDbContext.Orders.ToListAsync();
        }
        public override async Task<Order> GetById(int id)
        {
            string key = $"order-{id}";

            return await _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return ApplicationDbContext.Orders.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                });
        }

        public async Task<List<Order>> GetUserOrders(string userId)
        {
            return await ApplicationDbContext.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<List<Order>> GetExpiredOrders()
        {
            return await ApplicationDbContext.Orders.Where(o => o.CheckOut < DateTime.Now && o.Status != OrderStatus.Completed).ToListAsync();
        }
        public async Task<List<Order>> GetActiveOrders()
        {
            return await ApplicationDbContext.Orders.Where(o =>  o.CheckIn <= DateTime.Now && o.CheckOut >= DateTime.Now && o.Status == OrderStatus.Active).ToListAsync();
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
    }
}
