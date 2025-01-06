namespace Core.Contracts
{
    public record GetRoomsRequest
    (
        string? Search,
        string? SortItem,
        string? SortOrder,
        string? Categories,
        decimal? MinPrice,
        decimal? MaxPrice,
        DateTime? CheckIn,
        DateTime? CheckOut,
        int itemsCount = 9,
        int page = 1
    );
}
