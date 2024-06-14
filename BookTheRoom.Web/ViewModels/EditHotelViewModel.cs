using BookTheRoom.Domain.Entities;

namespace BookTheRoom.WebUI.ViewModels
{
    public class EditHotelViewModel : Hotel
    {
        public IFormFile? PreviewImage { get; set; }
        public List<IFormFile>? HotelImages { get; set; }
    }
}
