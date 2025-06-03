using Api.Contracts.Hotel;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Queries.Hotel;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IPhotoService _photoService;
        public HotelController(
            ISender sender,
            IPhotoService photoService)
        {
            _sender = sender;
            _photoService = photoService;
        }

        [HttpGet]        
        [AllowAnonymous]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> GetAll([FromQuery] GetHotelsRequest request)
        {
            var hotels = await _sender.Send(new GetHotelsQuery(request));

            var hotelsDTO = hotels.Select(h =>
                new HotelsDTO
                (
                    h.Id,
                    h.Name,

                    h.Images != null &&
                        h.Images.Any()
                        ? h.Images.First()
                        : "No Image",

                    h.Rating,
                    h.Address.ToString(true),

                    h.Comments != null && h.Comments.Any()
                        ? h.Comments
                            .Where(c => c.UserScore != null && c.UserScore > 0)
                            .Average(c => c.UserScore)
                        : -1f
                )
            ).ToList();

            return Ok(new GetHotelsResponse(hotelsDTO));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> Get(int id)
        {
            var hotel = await _sender.Send(new GetHotelQuery(id));

            if (hotel is null)
            {
                return NotFound($"Hotel with ID: {id} doesn't exist.");
            }

            var hotelDTO = new HotelDTO(
                hotel.Id,
                hotel.Name,
                hotel.Description,
                hotel.Address.ToString(),
                Address.AsJson(hotel.Address),
                hotel.HasPool,
                hotel.Rating,

                hotel.Comments != null && hotel.Comments.Any()
                        ? hotel.Comments
                            .Where(c => c.UserScore != null && c.UserScore > 0)
                            .Average(c => c.UserScore)
                        : -1f,

                hotel.Images != null && hotel.Images.Any()
                    ? hotel.Images
                    : new List<string> { "" },

                hotel.Comments ?? new List<Comment>()
                );            

            return Ok(hotelDTO);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Admin)]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Post([FromForm] CreateHotelForm form)
        {
            if (form.Images is not null && form.Images.Any() && form.Images.Count > 20)
            {
                return BadRequest("You cannot add more than 20 files.");
            }

            var images = new List<string>();           

            if (form.Images is not null && form.Images.Any())
            {
                foreach (var file in form.Images)
                {
                    using var stream = file.OpenReadStream();
                    var resultForList = await _photoService.AddPhotoAsync(file.Name, stream);
                    images.Add(resultForList.Url.ToString());
                }
            }

            var request = new CreateHotelRequest
            (
                form.Name,
                form.Description,
                form.Rating,
                form.Pool,
                new Address
                (
                    form.Country,
                    form.State,
                    form.City,
                    form.Street,
                    form.PostalCode
                ),
                images
            );

            var result = await _sender.Send(new CreateHotelCommand(request));

            if (!result.IsSuccess)
            {
                foreach (var image in images)
                {
                    await _photoService.DeletePhotoAsync(image);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Put(int id, [FromForm] UpdateHotelForm form)
        {
            if (form.Images is not null && form.Images.Any() && form.Images.Count > 20)
            {
                return BadRequest("You cannot add more than 20 files.");
            }

            var images = new List<string>();
            

            if (form.Images is not null && form.Images.Any())
            {               
                foreach (var file in form.Images)
                {
                    using var stream = file.OpenReadStream();
                    var resultForList = await _photoService.AddPhotoAsync(file.Name, stream);
                    images.Add(resultForList.Url.ToString());                
                }
            }

            var request = new UpdateHotelRequest
            (
                form.Name,
                form.Description,
                form.Rating,
                form.Pool,
                new Address
                (
                    form.Country,
                    form.State,
                    form.City,
                    form.Street,
                    form.PostalCode
                ),
                images.Any() 
                    ? images 
                    : null
            );            

            var result = await _sender.Send(new UpdateHotelCommand(id, request));

            if (!result.IsSuccess)
            {
                foreach (var image in images)
                {
                    await _photoService.DeletePhotoAsync(image);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sender.Send(new DeleteHotelCommand(id));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
