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

        public async Task<Dictionary<Order, (Hotel, Room)>> GetAllUserOrders(string userId, GetOrdersRequest request, CancellationToken token = default)
        {            
            var ordersQuery = context.Orders
                .Where(o => o.UserId == userId)
                .AsNoTracking();

            var hotelsQuery = context.Hotels
                .Include(h => h.Address)
                .Where(h => string.IsNullOrWhiteSpace(request.Search) ||
                    h.Name.ToLower().Contains(request.Search.ToLower()) ||
                    (h.Address != null && (
                        h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                        h.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                        h.Address.City.ToLower().Contains(request.Search.ToLower())
                    ))
                )
                .Select(h => h.Id);

            ordersQuery = ordersQuery.Where(o =>
                (string.IsNullOrWhiteSpace(request.Search) || o.Status.ToString().ToLower().Contains(request.Search.ToLower()))
                &&
                hotelsQuery.Contains(o.HotelId)
            );

            var query = from o in ordersQuery
                        join h in context.Hotels.Include(h => h.Address) on o.HotelId equals h.Id
                        join r in context.Rooms on new { o.HotelId, Number = o.RoomNumber } equals new { r.HotelId, r.Number }
                        select new { Order = o, Hotel = h, Room = r };

            query = request.SortOrder == "desc"
                ? request.SortItem?.ToLower() switch
                {
                    "price" => query.OrderByDescending(x => x.Order.OverallPrice),
                    "status" => query.OrderByDescending(x => (int)x.Order.Status),
                    "date" => query.OrderByDescending(x => x.Order.CreatedAt),
                    _ => query.OrderByDescending(x => x.Order.Id),
                }
                : request.SortItem?.ToLower() switch
                {
                    "price" => query.OrderBy(x => x.Order.OverallPrice),
                    "status" => query.OrderBy(x => (int)x.Order.Status),
                    "date" => query.OrderBy(x => x.Order.CreatedAt),
                    _ => query.OrderBy(x => x.Order.Id),
                };

            return await query.ToDictionaryAsync(x => x.Order, x => (x.Hotel, x.Room), token);
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
