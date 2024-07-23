namespace Core.Contracts
{
    public record GetRoomsRequest(
        string? Search,
        string? SortItem,
        string? SortOrder
        );
    
}
