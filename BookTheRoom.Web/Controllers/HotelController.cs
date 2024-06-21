using BookTheRoom.Application.Interfaces;
using BookTheRoom.Infrastructure.Identity;
using BookTheRoom.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using BookTheRoom.Application.UseCases.Queries.Hotel;
using BookTheRoom.Application.UseCases.Commands.Hotel;
using BookTheRoom.Application.UseCases.Commands.Address;



namespace BookTheRoom.WebUI.Controllers
{
    public class HotelController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IPhotoService _photoService;
        public HotelController(IMediator mediator, IPhotoService photoService)
        {
            _mediator = mediator;
            _photoService = photoService;
        }

        [AllowAnonymous]
        [EnableRateLimiting("fixed")]
        [Route("Hotels", Name = "Hotels")]
        public async Task<IActionResult> Hotels()
        {
            var hotels = await _mediator.Send(new GetAllHotelsQuery());
            return View(hotels);
        }

        [AllowAnonymous]
        [EnableRateLimiting("fixed")]
        [Route("Hotels/{id:int}", Name = "Hotel")]
        public async Task<IActionResult> Hotel([FromRoute] int id)
        {
            var hotel = await _mediator.Send(new GetHotelByIdQuery(id));
            if (hotel == null)
            {
                return Redirect($"/Hotels/{id}");
            }
            return View(hotel);
        }

        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        public IActionResult AddHotel()
        {
            return View();
        }


        [HttpPost]
        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/Add", Name = "AddHotel")]
        public async Task<IActionResult> AddHotel(AddHotelViewModel hotel)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(hotel.PreviewImage.Name,
                                                               hotel.PreviewImage.OpenReadStream());

                hotel.PreviewImage.OpenReadStream().Dispose();

                var hotelImages = new List<string>();

                foreach (var item in hotel.HotelImages)
                {
                    var resultForList = await _photoService.AddPhotoAsync(item.Name, item.OpenReadStream());
                    hotelImages.Add(resultForList.Url.ToString());
                    item.OpenReadStream().Dispose();
                }

                hotel.ImagesURL = hotelImages;
                hotel.PreviewURL = result.Url.ToString();

                await _mediator.Send(new CreateHotelCommand(hotel));

                return Redirect($"/Hotels");
            }
            return View(hotel);
        }


        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("/Hotels/{id:int}/Edit", Name = "EditHotel")]
        public async Task<IActionResult> EditHotel([FromRoute] int id)
        {
            var thisHotel = await _mediator.Send(new GetHotelByIdQuery(id));
            var editViewModel = new EditHotelViewModel
            {
                Id = id,
                Name = thisHotel.Name,
                Description = thisHotel.Description,
                Address = thisHotel.Address,
                HasPool = thisHotel.HasPool,
                NumberOfRooms = thisHotel.NumberOfRooms,
                Rating = thisHotel.Rating
            };
            return View(editViewModel);
        }


        [HttpPost]
        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("/Hotels/{id:int}/Edit", Name = "EditHotel")]
        public async Task<IActionResult> EditHotel([FromRoute] int id, EditHotelViewModel hotel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit hotel.");
                return View("EditHotel", hotel);
            }

            var thisHotel = await _mediator.Send(new GetHotelByIdQuery(id));

            var previewImage = thisHotel.PreviewURL;
            var hotelImages = thisHotel.ImagesURL;

            if (thisHotel != null)
            {
                if (hotel.PreviewImage != null)
                {
                    await _photoService.DeletePhotoAsync(thisHotel.PreviewURL);

                    var result = await _photoService.AddPhotoAsync(hotel.PreviewImage.Name,
                                                           hotel.PreviewImage.OpenReadStream());

                    hotel.PreviewImage.OpenReadStream().Dispose();
                    previewImage = result.Url.ToString();

                }

                if (!hotel.HotelImages.IsNullOrEmpty())
                {

                    foreach (var image in thisHotel.ImagesURL)
                    {

                        await _photoService.DeletePhotoAsync(image);
                    }

                    hotelImages.Clear();

                    foreach (var image in hotel.HotelImages)
                    {
                        var resultForList = await _photoService.AddPhotoAsync(image.Name, image.OpenReadStream());
                        image.OpenReadStream().Dispose();
                        hotelImages.Add(resultForList.Url.ToString());

                    }
                }
            }
            
            if (thisHotel.Address.Country != hotel.Address.Country ||
                thisHotel.Address.City != hotel.Address.City ||
                thisHotel.Address.StreetOrDistrict != hotel.Address.StreetOrDistrict ||
                thisHotel.Address.Index != hotel.Address.Index)
            {
                thisHotel.Address = hotel.Address;
            }

            hotel.ImagesURL = hotelImages;
            hotel.PreviewURL = previewImage;


            await _mediator.Send(new UpdateHotelCommand(hotel));

            return Redirect($"/Hotels/{id}");
        }


        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/{id:int}/Delete", Name = "DeleteHotel")]
        public async Task<IActionResult> DeleteHotel([FromRoute] int id)
        {
            var hotelToDelete = await _mediator.Send(new GetHotelByIdQuery(id));
            if (hotelToDelete == null) { return View("Error"); }

            return View(hotelToDelete);
        }


        [HttpPost, ActionName("DeleteHotel")]
        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/{id:int}/Delete", Name = "DeleteHotel")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var hotelToDelete = await _mediator.Send(new GetHotelByIdQuery(id));
            if (hotelToDelete != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(hotelToDelete.PreviewURL);

                    foreach (var image in hotelToDelete.ImagesURL)
                    {
                        await _photoService.DeletePhotoAsync(image);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Couldn't delete the photo.");
                    return View(hotelToDelete);
                }
            }
            if (hotelToDelete == null) { return View("Error"); }

            await _mediator.Send(new DeleteAddressCommand(hotelToDelete.Address));
            await _mediator.Send(new DeleteHotelCommand(hotelToDelete));
            return Redirect("/Hotels");
        }       
    }
}
