using Application.UseCases.Queries.Room;
using System.Linq;

namespace Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetHotelRoomsQueryHandler(IUnitOfWork unitOfWork, IValidator<GetHotelRoomsQuery> validator) : IQueryHandler<GetHotelRoomsQuery, List<Core.Entities.Room>>
    {
        public async Task<List<Core.Entities.Room>> Handle(GetHotelRoomsQuery query, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var allOrders = await unitOfWork.Orders.GetAll(new GetOrdersRequest(null, null, null)); // все заказы
                                                                           
            var hotelRooms = await unitOfWork.Rooms.GetAll(query.HotelId, query.Request); // все комнаты данного отеля

            if (allOrders is null || !allOrders.Any())
            {
                return hotelRooms;
            }

            var roomsToExclude = allOrders
                .Where(o => 
                    o.HotelId == query.HotelId &&
                    (query.CheckIn <= o.CheckIn && query.CheckOut >= query.CheckIn) ||
                    (query.CheckIn <= o.CheckOut && query.CheckOut >= o.CheckOut)
                )
                .Select(o => o.RoomNumber)
                .ToList(); // брони связанные с даным отелем на время, указанное в фильтрах, выбор номеров, соответствующих этому условию           

            var freeRooms = hotelRooms
                .Where(x => !roomsToExclude.Contains(x.Number)).ToList();

            return freeRooms;
        }
    }
}