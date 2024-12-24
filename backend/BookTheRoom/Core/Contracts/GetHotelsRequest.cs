namespace Core.Contracts
{
    public record GetHotelsRequest
    (        
        string? Search,
        string? SortItem,
        string? SortOrder,
        string? Countries,
        string? Ratings,
        string? Services,
        int ItemsCount = 15,
        int Page = 1
    );
}
