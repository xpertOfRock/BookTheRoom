
namespace Core.Contracts
{
    public record GetUserOrdersRequest(
        string? Search,
        string? SortItem,
        string? SortOrder
        );
}
