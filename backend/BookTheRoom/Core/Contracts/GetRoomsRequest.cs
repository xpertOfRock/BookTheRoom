namespace Core.Contracts
{
    public record GetRoomsRequest
    (
        string? Search,
        string? SortItem,
        string? SortOrder,
        string? Categories,
        string? Prices,
        int itemsCount = 9,
        int page = 1
    );
}
