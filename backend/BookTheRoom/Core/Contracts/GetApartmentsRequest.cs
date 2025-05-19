namespace Core.Contracts
{
    public record GetApartmentsRequest
    (
        string? Search,
        string? SortItem,
        string? SortOrder,
        string? Countries,
        decimal? MinPrice,
        decimal? MaxPrice,
        int ItemsCount = 9,
        int Page = 1
    );
}
