using Core.Entities;

namespace Core.Contracts
{
    public record UpdateHotelRequest(
        string Name,
        string Description,
        int Rating,
        int RoomsAmount,
        bool HasPool,
        List<string> Images,
        List<Room> Rooms);
}
