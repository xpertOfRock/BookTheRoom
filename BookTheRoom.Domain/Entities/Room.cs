using BookTheRoom.Domain.Enums;

namespace BookTheRoom.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public decimal PriceForRoom { get; set; }          
        public bool IsFree { get; set; }       
        public int HotelId { get; set; }  
        public int PreviewImageId { get; set; }

        public RoomImage? PreviewImage { get; set; }
        public Hotel Hotel { get; set; }
        public RoomCategory RoomCategory { get; set; }
        public ICollection<RoomImage>? RoomImages { get; set; }
    }
}
