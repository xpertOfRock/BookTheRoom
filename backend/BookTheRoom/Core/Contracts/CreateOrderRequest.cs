using Core.Enums;

namespace Core.Contracts
{
    public record CreateOrderRequest
    (
        string NonceFromClient,
        string Email,
        string Number,
        bool MinibarIncluded,
        bool MealsIncluded,        
        DateTime CheckIn,
        DateTime CheckOut,
        DateTime CreatedAt,
        OrderStatus Status,
        bool PaidImmediately = false
    );
}