using Api.DTOs;

namespace Api.Contracts.Room
{
    public record GetRoomsResponse(List<RoomsDTO> Rooms);
}
