using Core.Enums;

namespace Api.DTOs
{
    public record UserOrdersDTO
    (
        int OrderId,
        string HotelName,
        int RoomNumber,
        decimal OverallPrice,
        bool MinibarIncluded,
        bool MealsIncluded,
        DateTimeOffset CheckIn,
        DateTimeOffset CheckOut,
        string Address,
        OrderStatus Status
    );
}
