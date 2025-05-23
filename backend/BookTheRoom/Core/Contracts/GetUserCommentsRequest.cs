namespace Core.Contracts
{
    public record GetUserCommentsRequest
    (
        string? Search,
        string? SortItem,
        string? SortOrder,
        int Page = 1,
        int ItemsCount = 9
    );
}
