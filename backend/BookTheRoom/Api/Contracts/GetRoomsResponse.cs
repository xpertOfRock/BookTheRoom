using Api.DTOs;

namespace Api.Contracts
{
    public record GetRoomsResponse(List<RoomsDTO> Rooms);
}
