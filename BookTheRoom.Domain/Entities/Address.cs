using System.ComponentModel.DataAnnotations;

namespace BookTheRoom.Domain.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
