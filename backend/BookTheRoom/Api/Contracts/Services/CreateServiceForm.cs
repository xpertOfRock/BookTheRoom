using Core.Enums;

namespace Api.Contracts.Services
{
    public record CreateServiceForm
    (
        HotelService ServiceName,
        string Description,
        decimal Price 
    );
}
