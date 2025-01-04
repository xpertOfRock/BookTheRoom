using Api.Contracts.Comment;
using Api.Contracts.Hotel;
using Api.DTOs;
using Api.Extensions;
using Application.Interfaces;
using Application.UseCases.Commands.Comment;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Queries.Hotel;
using Core.Contracts;
using Core.Entities;
using Core.ValueObjects;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _contextAccessor;
        public HotelController(IMediator mediator, IPhotoService photoService, IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _photoService = photoService;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] GetHotelsRequest request)
        {
            var hotels = await _mediator.Send(new GetHotelsQuery(request));

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
                        ? h.Comments.Average(c => c.UserScore)
                        : -1f
                )
            ).ToList();

            return Ok(new GetHotelsResponse(hotelsDTO));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var hotel = await _mediator.Send(new GetHotelQuery(id));

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
                        ? hotel.Comments.Average(c => c.UserScore)
                        : -1f,

                hotel.Images != null &&
                    hotel.Images.Any()
                    ? hotel.Images
                    : new List<string> { "" },

                hotel.Comments != null &&
                    hotel.Comments.Any()
                    ? hotel.Comments
                    : new List<Comment> { }                    
                );            

            return Ok(hotelDTO);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Post([FromForm] CreateHotelForm form)
        {
            var imagesUrl = new List<string>();

            if (form.Images is not null && form.Images.Any())
            {
                foreach (var file in form.Images)
                {
                    using var stream = file.OpenReadStream();
                    var resultForList = await _photoService.AddPhotoAsync(file.Name, stream);
                    imagesUrl.Add(resultForList.Url.ToString());
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
                imagesUrl
            );

            var result = await _mediator.Send(new CreateHotelCommand(request));

            return result.IsSuccess ? Created() : BadRequest(result);
        }
        //[HttpPost("{id}/services")]
        //[Authorize(Roles = UserRole.Admin)]
        //public async Task<IActionResult> PostService(int hotelId, [FromBody] )
        //{
        //    return Ok();
        //}

        [HttpPost("{id}/comments")]
        [Authorize]
        public async Task<IActionResult> PostComment(int id, [FromBody] CreateCommentForm form)
        {
            var userId = _contextAccessor.HttpContext?.User.GetUserId();
            var username = _contextAccessor.HttpContext?.User.GetUsername();

            if(userId is null || username.IsNullOrEmpty())
            {
                return Unauthorized();
            }

            var result = await _mediator.Send(new CreateCommentCommand(userId, username!, form.Description, form.UserScore, id));

            return result.IsSuccess ? Created() : BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Put(int id, [FromForm] UpdateHotelForm form)
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

            var result = await _mediator.Send(new UpdateHotelCommand(id, request));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteHotelCommand(id));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
