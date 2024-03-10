using BookTheRoom.Domain.Entities;

namespace BookTheRoom.WebUI.ViewModels
{
    public class CreateHotelViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfRooms { get; set; }
        public int Rating { get; set; }
        public bool HasPool { get; set; }

        public IFormFile PreviewImage { get; set; }
        public Address Address { get; set; }
    }
}
