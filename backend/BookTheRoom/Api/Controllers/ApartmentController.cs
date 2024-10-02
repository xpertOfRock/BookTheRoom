using Api.Contracts;
using Api.DTOs;
using Api.Extensions;
using Application.Interfaces;
using Application.UseCases.Commands.Apartment;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Queries.Apartment;
using Core.Contracts;
using Core.Entities;
using Core.ValueObjects;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApartmentController
        (
            IMediator mediator,
            IPhotoService photoService,
            IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager
        )
        {
            _mediator = mediator;
            _photoService = photoService;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("user")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsersApartments([FromQuery] GetApartmentsRequest request)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("You have to sign in to add an apartment for rent.");
            }

            var thisUserId = _contextAccessor.HttpContext?.User.GetUserId();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == thisUserId);

            var apartments = await _mediator.Send(new GetUsersApartmentsQuery(thisUserId, request));

            var apartmentsDTO = apartments.Select(h => new ApartmentsDTO(
                h.Id,
                h.Title,
                h.PriceForNight,
                h.Address.ToString(),
                h.Images != null &&
                    h.Images.Any()
                    ? h.Images.First()
                    : "No Image"
                )
            ).ToList();

            return Ok(new GetApartmentsResponse(apartmentsDTO));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] GetApartmentsRequest request)
        {
            var apartments = await _mediator.Send(new GetApartmentsQuery(request));

            var apartmentsDTO = apartments.Select(h => new ApartmentsDTO(
                h.Id,
                h.Title,
                h.PriceForNight,
                h.Address.ToString(),

                h.Images != null &&
                    h.Images.Any()
                    ? h.Images.First()
                    : "No Image"
                )
            ).ToList();

            return Ok(new GetApartmentsResponse(apartmentsDTO));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var apartment = await _mediator.Send(new GetApartmentQuery(id));

            if (apartment is null)
            {
                return NotFound($"Hotel with ID: {id} doesn't exist.");
            }

            var thisUserId = _contextAccessor.HttpContext?.User.GetUserId();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == thisUserId);

            var apartmentDTO = new ApartmentDTO(
                apartment.Id,
                apartment.Title,
                apartment.Description,
                $"{user.FirstName} {user.LastName}",
                apartment.Address.ToString(),

                apartment.Images != null &&
                    apartment.Images.Any()
                    ? apartment.Images
                    : new List<string> { "" },

                apartment.Comments != null &&
                    apartment.Comments.Any()
                    ? apartment.Comments
                    : new List<Comment> { }
                );

            return Ok(apartmentDTO);
        }

        [HttpPost]
        //[Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Post([FromForm] CreateApartmentForm form)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("You have to sign in to add an apartment for rent.");
            }
            var thisUserId = _contextAccessor.HttpContext?.User.GetUserId();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == thisUserId);

            var imagesUrl = new List<string>();

            if (form.Images is not null && form.Images.Any())
            {
                foreach (var file in form.Images)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var resultForList = await _photoService.AddPhotoAsync(file.Name, stream);
                        imagesUrl.Add(resultForList.Url.ToString());
                    }
                }
            }

            var request = new CreateApartmentRequest
            (
                form.Title,
                form.Description,
                thisUserId,
                user.FirstName + " " + user.LastName,
                form.PricePerNight,
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

            await _mediator.Send(new CreateApartmentCommand(request));
            return Ok();
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Put(int id, [FromForm] UpdateHotelForm form)
        {
            var images = new List<string>();

            if (form.Images is not null && form.Images.Any())
            {
                foreach (var file in form.Images)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var resultForList = await _photoService.AddPhotoAsync(file.Name, stream);
                        images.Add(resultForList.Url.ToString());
                    }
                }
            }

            var request = new UpdateHotelRequest
            (
                form.Name,
                form.Description,
                form.Rating,
                form.Pool,
                images,
                new List<Comment>()
            );

            await _mediator.Send(new UpdateHotelCommand(id, request));
            return Ok();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteHotelCommand(id));
            return Ok();
        }
    }
}
