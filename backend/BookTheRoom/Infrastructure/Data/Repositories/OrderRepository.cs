namespace Infrastructure.Data.Repositories
{
    public class OrderRepository(
        ApplicationDbContext context,
        IDistributedCache distributedCache) : IOrderRepository
    {
        public async Task<IResult> Add(Order order)
        {
            await context.Orders.AddAsync(order);
            return new Success("Entity 'Order' was created successfully.");
        }

        public async Task<List<Order>?> GetActiveOrders()
        {
            var activeOrders = await context.Orders
                     .Where(o => o.Status == OrderStatus.Active)
                     .AsNoTracking()
                     .ToListAsync();

            return activeOrders;
        }

        public async Task<List<Order>> GetAll(GetOrdersRequest request)
        {
            var query = context.Orders
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

        public async Task<List<Order>?> GetAllUserOrders(string userId, GetDataRequest request)
        {
            var query = context.Orders
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
            return await context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>?> GetExpiredOrders()
        {
            var expiredOrders = await context.Orders
                .Where(o =>
                       o.CheckOut < DateTime.UtcNow &&
                       o.Status != OrderStatus.Completed)
                .AsNoTracking()
                .ToListAsync();

            return expiredOrders;
        }

        public async Task Update(int id, UpdateOrderRequest request)
        {
            string key = $"order-{id}";

            distributedCache.Remove(key);

            await context.Orders
                .Where(o => o.Id == id)
                .ExecuteUpdateAsync(e => e
                .SetProperty(o => o.Status, request.Status)
                );            
        }
    }
}
