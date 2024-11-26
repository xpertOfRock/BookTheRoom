namespace Core.Contracts
{
    public record GetRoomsRequest
    (
        string? Search,
        string? SortItem,
        string? SortOrder,
        string? Categories,
        string? Prices,
        int page = 1
    );
}
