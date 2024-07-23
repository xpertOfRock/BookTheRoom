namespace Core.Contracts
{
    public record GetOrdersRequest(
        string? Search,
        string? SortItem,
        string? SortOrder
        );
}
