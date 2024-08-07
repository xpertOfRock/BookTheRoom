using Core.ValueObjects;

namespace Api.DTOs
{
    public record HotelsDTO(
        int Id,
        string Name,
        string? Preview,
        int Rating,
        string Address
        );
}
