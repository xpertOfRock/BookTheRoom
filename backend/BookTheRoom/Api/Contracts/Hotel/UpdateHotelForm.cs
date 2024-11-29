using Core.Enums;

namespace Api.Contracts.Hotel
{
    public record UpdateHotelForm
    (
        string Name,
        string Description,
        uint Rating,
        bool Pool,
        List<IFormFile>? Images
    );
}
