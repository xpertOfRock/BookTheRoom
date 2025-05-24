namespace Application.DTOs
{
    public record UserOrdersDTO
    (
        int OrderId,
        string HotelName,
        int RoomNumber,
        decimal OverallPrice,
        bool MinimarIncluded,
        bool MealsIncluded,
        DateTimeOffset CheckIn,
        DateTimeOffset CheckOut,
        string Address,
        OrderStatus Status
    );
}
