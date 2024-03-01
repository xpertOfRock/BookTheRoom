using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTheRoom.Domain.Entities
{
    public class Hotel
    {     
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public bool HasPool { get; set; }
        public int AddressId { get; set; }

        public Address Address { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
