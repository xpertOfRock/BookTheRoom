namespace Api.Contracts.Order
{
    public record GetUserOrdersResponse(List<UserOrdersDTO> Orders);
}
