using Core.ValueObjects;

namespace Core.Contracts
{
    public record CreateHotelRequest
    (
        string Name,
        string Description,
        int Rating,
        int Rooms, // stands for total amount of rooms in the hotel instead of List<Room> 
        bool Pool, // defines either hotel have pool or not
        Address Address, //JSON string for deserialization to format Core.ValueObjects.Address
        List<string> Images // contains URLs of the images that were added to the Cloudinary
    );
}