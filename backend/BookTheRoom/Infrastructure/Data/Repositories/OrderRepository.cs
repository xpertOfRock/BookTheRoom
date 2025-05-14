using Infrastructure.Exceptions;

namespace Infrastructure.Data.Repositories
{
    public class OrderRepository(
        ApplicationDbContext context,
        IDistributedCache distributedCache) : IOrderRepository
    {
        public async Task<IResult> Add(Order order, CancellationToken token = default)
        {
            await context.Orders.AddAsync(order, token);
            return new Success("Entity 'Order' was created successfully.");
        }

        public async Task<List<Order>?> GetActiveOrders(CancellationToken token = default)
        {
            return await context.Orders
                     .Where(o => o.Status == OrderStatus.Active)
                     .AsNoTracking()
                     .ToListAsync(token);
        }

        public async Task<List<Order>> GetAll(GetOrdersRequest request, CancellationToken token = default)
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


            return await query.ToListAsync(token);
        }

        public async Task<List<Order>?> GetAllUserOrders(string userId, GetDataRequest request, CancellationToken token = default)
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


            return await query.ToListAsync(token);
        }

        public async Task<Order> GetById(int orderId, CancellationToken token = default)
        {
            var order = await context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId, token);

            if(order is null)
            {
                throw new EntityNotFoundException<Order>();
            }

            return order;
        }

        public async Task<List<Order>?> GetExpiredOrders(CancellationToken token = default)
        {
            var expiredOrders = await context.Orders
                .Where(o =>
                       o.CheckOut < DateTime.UtcNow &&
                       o.Status != OrderStatus.Completed)
                .AsNoTracking()
                .ToListAsync(token);

            return expiredOrders;
        }

        public async Task Update(int id, UpdateOrderRequest request, CancellationToken token = default)
        {
            string key = $"order-{id}";

            await distributedCache.RemoveAsync(key, token);

            await context.Orders
                .Where(o => o.Id == id)
                .ExecuteUpdateAsync(e => e
                .SetProperty(o => o.Status, request.Status),
                token);            
        }
    }
}
