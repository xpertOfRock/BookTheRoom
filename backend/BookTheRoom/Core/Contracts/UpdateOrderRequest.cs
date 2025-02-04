using Core.Enums;

namespace Core.Contracts
{
    public record UpdateOrderRequest
    (
        OrderStatus Status
    );
}
