using Core.Contracts;

namespace Api.Contracts
{
    public record CreateHotelForm(CreateHotelRequest Request, List<IFormFile>? Files);
}
