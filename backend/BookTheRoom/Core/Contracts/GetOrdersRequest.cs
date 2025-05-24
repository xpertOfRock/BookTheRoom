namespace Core.Contracts
{
    public record GetOrdersRequest
    (
        string? Search,
        string? SortItem,
        string? SortOrder,
        int Page = 1,
        int ItemsCount = 9
    );
}
