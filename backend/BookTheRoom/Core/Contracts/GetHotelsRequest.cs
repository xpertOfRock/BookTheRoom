namespace Core.Contracts
{
    public record GetHotelsRequest(
        string? Search,
        string? SortItem,
        string? SortOrder
        );
}
