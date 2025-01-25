using Application.UseCases.Queries.Room;

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

            var activeOrders = await unitOfWork.Orders.GetActiveOrders();
                                                                           
            var hotelRooms = await unitOfWork.Rooms.GetAll(query.HotelId, query.Request);

            if (activeOrders is null || !activeOrders.Any())
            {
                return hotelRooms;
            }

            activeOrders = activeOrders.Where(o => o.HotelId == query.HotelId).ToList();                      

            var freeRooms = activeOrders
                .Where(x => 
                    query.CheckIn > x.CheckOut && 
                    query.CheckOut < x.CheckIn 
                )
                .Select(x => x.RoomNumber).ToList();

            return hotelRooms.Where(x => freeRooms.Contains(x.Number)).ToList();
        }
    }
}