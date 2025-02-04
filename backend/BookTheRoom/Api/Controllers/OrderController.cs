using Api.Extensions;
using Application.UseCases.Commands.Order;
using Application.UseCases.Queries.Order;
using Core.Contracts;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController
        (
            IMediator mediator,
            IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager
        )
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }
        [HttpGet("client-token")]
        public async Task<IActionResult> GetClientToken()
        {
            var token = await _mediator.Send(new GetClientTokenQuery());
            return Ok(token);
        }

        [HttpPost("{hotelId}/{number}")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(int hotelId, int number, [FromBody] CreateOrderRequest request)
        {
            string? userId = _contextAccessor.HttpContext!.User.GetUserId() ?? null;           
                     
            var result = await _mediator.Send(new CreateOrderCommand(hotelId, number, userId, request));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
