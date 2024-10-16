using Core.Contracts;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public async Task Add(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<List<Order>> GetActiveOrders()
        {
            return await _context.Orders
                     .Where(o =>
                            o.CheckIn <= DateTime.UtcNow &&
                            o.CheckOut >= DateTime.UtcNow &&
                            (o.Status == OrderStatus.Active || o.Status == OrderStatus.Pending))
                     .AsNoTracking()
                     .ToListAsync();
        }

        public async Task<List<Order>> GetAll(GetDataRequest request)
        {
            var query = _context.Orders
                .Where(o => string.IsNullOrWhiteSpace(request.Search) ||
                            o.Status.ToString().ToLower().Contains(request.Search.ToLower()))
                .AsNoTracking();

            Expression<Func<Order, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "price" => order => order.OverallPrice,
                "status" => order => order.Status.ToString(),
                "date" => order => order.CreatedAt,
                _ => order => order.Id,
            };

            query = request.SortOrder == "desc"
                 ? query = query.OrderByDescending(selectorKey)
                 : query = query.OrderBy(selectorKey);


            return await query.ToListAsync();
        }

        public async Task<List<Order>> GetAllUserOrders(string userId, GetDataRequest request)
        {
            var query = _context.Orders
                .Where(o => o.UserId == userId && (
                            string.IsNullOrWhiteSpace(request.Search) ||                            
                            o.Status.ToString().ToLower().Contains(request.Search.ToLower())
                            )
                       )
                .AsNoTracking();

            Expression<Func<Order, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "price" => order => order.OverallPrice,
                "status" => order => order.Status.ToString(),
                "date" => order => order.CreatedAt,
                _ => order => order.Id,
            };

            if (request.SortOrder == "desc")
            {
                query = query.OrderByDescending(selectorKey);
            }
            else
            {
                query = query.OrderBy(selectorKey);
            }


            return await query.ToListAsync();
        }

        public async Task<Order> GetById(int orderId)
        {
            return await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>> GetExpiredOrders()
        {
            return await _context.Orders
                .Where(o =>
                       o.CheckOut < DateTime.UtcNow &&
                       o.Status != OrderStatus.Completed)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Update(int id, UpdateOrderRequest request)
        {
            string key = $"order-{id}";

            _memoryCache.Remove(key);

            await _context.Orders
                .Where(o => o.Id == id)
                .ExecuteUpdateAsync(e => e
                .SetProperty(o => o.Status, request.Status)
                .SetProperty(o => o.MinibarIncluded, request.MinibarIncluded)
                .SetProperty(o => o.MealsIncluded, request.MealsIncluded)
                );            
        }
    }
}
