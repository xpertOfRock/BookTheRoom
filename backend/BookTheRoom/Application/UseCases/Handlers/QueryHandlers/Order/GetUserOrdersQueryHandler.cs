using Application.DTOs;
using Application.UseCases.Queries.Order;

namespace Application.UseCases.Handlers.QueryHandlers.Order
{
    public class GetUserOrdersQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetUserOrdersQuery, List<UserOrdersDTO>>
    {
        public async Task<List<UserOrdersDTO>> Handle(GetUserOrdersQuery query, CancellationToken cancellationToken)
        {
            var ordersDict = await unitOfWork.Orders.GetAllUserOrders(query.UserId, query.Filters, cancellationToken);

            if (ordersDict == null || ordersDict.Count == 0)
                return new List<UserOrdersDTO>();

            var dtos = ordersDict.Select(kvp =>
            {
                var order = kvp.Key;
                var hotel = kvp.Value.Item1;
                var room = kvp.Value.Item2;

                string address = hotel.Address != null
                    ? hotel.Address.ToString(true)
                    : string.Empty;

                return new UserOrdersDTO(
                    OrderId: order.Id,
                    HotelName: hotel.Name,
                    RoomNumber: room.Number,
                    OverallPrice: order.OverallPrice,
                    MinimarIncluded: order.MinibarIncluded,
                    MealsIncluded: order.MealsIncluded,
                    CheckIn: order.CheckIn.ToUniversalTime(),
                    CheckOut: order.CheckOut,
                    Address: address
                );
            }).ToList();

            dtos
                .Skip((query.Filters.Page - 1) * query.Filters.ItemsCount)
                .Take(query.Filters.ItemsCount);

            return dtos;
        }
    }
}
