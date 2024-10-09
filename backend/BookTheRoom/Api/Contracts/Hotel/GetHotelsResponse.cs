using Api.DTOs;

namespace Api.Contracts.Hotel
{
    public record GetHotelsResponse(List<HotelsDTO> Hotels);
}
