using BookTheRoom.Domain.Entities;

namespace BookTheRoom.WebUI.ViewModels
{
    public class AddHotelViewModel : Hotel
    {        
        public IFormFile PreviewImage { get; set; }
        public List<IFormFile> HotelImages { get; set; }

    }
}
