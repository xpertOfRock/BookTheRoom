using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTheRoom.Domain.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public double OverallPrice { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public Hotel Hotel { get; set; }
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }   
        public Room Room { get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }
    }
}
