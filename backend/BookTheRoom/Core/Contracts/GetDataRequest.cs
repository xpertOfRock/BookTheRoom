namespace Core.Contracts
{
    public record GetDataRequest
    (
        string? Search,
        string? SortItem,
        string? SortOrder
    );
}
