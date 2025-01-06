using Application.UseCases.Queries.Room;

namespace Application.UseCases.Handlers.QueryHandlers.Room
{
    public class GetHotelRoomsQueryHandler : IQueryHandler<GetHotelRoomsQuery, List<Core.Entities.Room>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<GetHotelRoomsQuery> _validator;
        public GetHotelRoomsQueryHandler(IUnitOfWork unitOfWork, IValidator<GetHotelRoomsQuery> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<List<Core.Entities.Room>> Handle(GetHotelRoomsQuery query, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var activeOrders = await _unitOfWork.Orders.GetActiveOrders();
                                                                           
            var hotelRooms = await _unitOfWork.Rooms.GetAll(query.HotelId, query.Request);

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