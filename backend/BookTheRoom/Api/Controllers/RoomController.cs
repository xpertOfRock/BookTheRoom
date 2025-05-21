using Api.Contracts.Room;
using Application.UseCases.Commands.Room;
using Application.UseCases.Queries.Room;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IPhotoService _photoService;
        public RoomController(
            ISender sender,
            IPhotoService photoService)
        {
            _sender = sender;
            _photoService = photoService;
        }

        [HttpGet("{hotelId}")]
        [AllowAnonymous]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> GetAll(int hotelId, [FromQuery] GetRoomsRequest request)
        {            
            var rooms = await _sender.Send(new GetHotelRoomsQuery(hotelId, request));

            var roomsDTO = rooms.Select(r => new RoomsDTO(
                r.HotelId,
                r.Name,
                r.Number,
                r.Price,
                r.Images != null &&
                    r.Images.Any()
                    ? r.Images.First()
                    : "No Image",
                r.Category
                )
            ).ToList();
            return Ok(new GetRoomsResponse(roomsDTO));
        }

        [HttpGet("{hotelId}/{number}")]
        [AllowAnonymous]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> Get(int hotelId, int number)
        {
            var room = await _sender.Send(new GetRoomQuery(hotelId, number));

            if(room is null)
            {
                return NotFound("This room doesn't exist.");
            }

            var roomDTO = new RoomDTO(
                room.HotelId,
                room.Name,
                room.Description,
                room.Number,
                room.Price,

                room.Images != null &&
                    room.Images.Any()
                    ? room.Images
                    : new List<string> { },

                room.Category
                );

            return Ok(roomDTO); 
        }
       
        [HttpPost("{hotelId}")]
        [Authorize(Roles = UserRole.Admin)]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Post(int hotelId, [FromForm] CreateRoomForm form)
        {
            var images = new List<string>();

            if (form.Images.Any())
            {
                foreach (var file in form.Images)
                {
                    using var stream = file.OpenReadStream();
                    var resultForList = await _photoService.AddPhotoAsync(file.Name, stream);
                    images.Add(resultForList.Url.ToString());
                }
            }

            var request = new CreateRoomRequest
            (
                form.Title,
                form.Description,
                form.Number,
                form.PricePerNight,
                (Core.Enums.RoomCategory)form.Category,
                images
            );

            var result = await _sender.Send(new CreateRoomCommand(hotelId, request));

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

        [HttpPut("{hotelId}/{number}")]
        [Authorize(Roles = UserRole.Admin)]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Put(int hotelId, int number, [FromForm] UpdateRoomForm form)
        {
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

            var request = new UpdateRoomRequest
            (
                form.Name,
                form.Description,
                form.Price,
                form.Category,
                images
            );

            var result = await _sender.Send(new UpdateRoomCommand(hotelId, number, request));

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

        [HttpDelete("{hotelId}/{number}")]
        [Authorize(Roles = UserRole.Admin)]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Delete(int hotelId, int number)
        {
            var result = await _sender.Send(new DeleteRoomCommand(hotelId, number));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
