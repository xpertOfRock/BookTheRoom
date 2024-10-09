using Core.Enums;

namespace Api.Contracts.Hotel
{
    public record UpdateHotelForm
    (
        string Name,
        string Description,
        int Rating,
        bool Pool,
        List<IFormFile>? Images
    );
}
