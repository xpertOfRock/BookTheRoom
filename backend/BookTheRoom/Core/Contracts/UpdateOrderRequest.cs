using Core.Enums;

namespace Core.Contracts
{
    public record UpdateOrderRequest(bool MinibarIncluded, bool MealsIncluded, OrderStatus Status);
}
