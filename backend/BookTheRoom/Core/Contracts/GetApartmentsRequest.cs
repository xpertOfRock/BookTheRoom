namespace Core.Contracts
{
    public record GetApartmentsRequest
    (
        string? Search,
        string? SortItem,
        string? SortOrder,
        string? Countries,
        string? Prices,
        int page = 1
    );
}
