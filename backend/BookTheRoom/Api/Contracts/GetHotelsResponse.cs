using Api.DTOs;

namespace Api.Contracts
{
    public record GetHotelsResponse(List<HotelsDTO> Hotels);
}
