using Api.Contracts;
using Api.DTOs;
using Application.Interfaces;
using Application.UseCases.Commands.Room;
using Application.UseCases.Queries.Room;
using Core.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPhotoService _photoService;
        public RoomController(IMediator mediator, IPhotoService photoService)
        {
            _mediator = mediator;
            _photoService = photoService;
        }

        [HttpGet("{hotelId}")]
        public async Task<IActionResult> GetAll(int hotelId, [FromQuery] GetRoomsRequest request)
        {            
            var rooms = await _mediator.Send(new GetHotelRoomsQuery(hotelId, request));

            var roomsDTO = rooms.Select(r => new RoomsDTO(
                r.HotelId,
                r.Name,
                r.Number,
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
        public async Task<IActionResult> Get(int hotelId, int number)
        {
            var room = await _mediator.Send(new GetRoomQuery(hotelId, number));

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
        public async Task<IActionResult> Post(int hotelId, [FromBody] CreateRoomRequest request)
        {
            await _mediator.Send(new CreateRoomCommand(hotelId, request));
            return Ok();
        }

        [HttpPut("{hotelId}/{number}")]
        public async Task<IActionResult> Put(
            int hotelId,
            int number,
            [FromBody] UpdateRoomRequest request,
            [FromForm] List<IFormFile>? files)
        {
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    var resultForList = await _photoService.AddPhotoAsync(file.Name, file.OpenReadStream());
                    request.Images.Add(resultForList.Url.ToString());
                    file.OpenReadStream().Dispose();
                }
            }
            await _mediator.Send(new UpdateRoomCommand(hotelId, number, request));
            return Ok();
        }

        [HttpDelete("{hotelId}/{number}")]
        public async Task<IActionResult> Delete(int hotelId, int number)
        {
            await _mediator.Send(new DeleteRoomCommand(hotelId, number));
            return Ok();
        }
    }
}
