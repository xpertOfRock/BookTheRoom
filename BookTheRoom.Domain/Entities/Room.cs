using BookTheRoom.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookTheRoom.Domain.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public double PriceForRoom { get; set; }
        public Hotel Hotel { get; set; }
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public bool IsFree { get; set; }
        public RoomCategory RoomCategory { get; set; }
    }
}
