using Api.Contracts.Comment;
using Api.Extensions;
using Application.UseCases.Commands.Comment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Controllers
{
    /// <summary>
    /// Controller for posting comments to hotels or apartments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IHttpContextAccessor _contextAccessor;

        public CommentController(
            ISender sender,
            IHttpContextAccessor contextAccessor)
        {
            _sender = sender;
            _contextAccessor = contextAccessor;
        }
        /// <summary>
        /// Posts a comment for a property (hotel/apartment).
        /// </summary>
        /// <param name="form">The comment form containing property details and user feedback.</param>
        [HttpPost]
        [Authorize]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> PostComment([FromBody] CreateCommentForm form)
        {
            var userId = _contextAccessor.HttpContext?.User.GetUserId();
            var username = _contextAccessor.HttpContext?.User.GetUsername();

            if (userId is null || string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            var result = await _sender.Send(new CreateCommentCommand(userId, username!, form.Description, form.PropertyId, form.PropertyType,form.UserScore));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
