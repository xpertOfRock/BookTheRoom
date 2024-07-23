using Core.ValueObjects;

namespace Core.Contracts
{
    public record CreateHotelRequest(
        string Name,
        string Description,
        int Rating,
        int RoomsAmount,
        bool HasPool,
        Address Address
        );
}