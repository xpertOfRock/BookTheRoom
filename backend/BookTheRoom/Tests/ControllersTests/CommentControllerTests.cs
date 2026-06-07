using Api.Contracts.Comment;
using Api.Controllers;
using Application.UseCases.Commands.Comment;
using Application.UseCases.Queries.Comment;
using Core.Contracts;
using Core.Enums;
using Core.TasksResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tests.ControllersTests
{
    // Unit tests for CommentController.
    // Covers: missing claims → 401, happy path → 200/400, GetUserComments routing.
    public class CommentControllerTests
    {
        private readonly Mock<IMediator> _sender = new();

        private CommentController BuildController(string? userId = "user-001",
                                                   string? username = "johndoe")
        {
            var claims = new List<Claim>();
            if (userId != null)
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            if (username != null)
                claims.Add(new Claim(ClaimTypes.Name, username));

            var identity = new ClaimsIdentity(claims,
                (userId != null) ? "TestAuth" : string.Empty);
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            var controller = new CommentController(_sender.Object, accessorMock.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            return controller;
        }

        private static CreateCommentForm ValidForm() =>
            new CreateCommentForm(PropertyId: 1, PropertyType: PropertyType.Hotel,
                Description: "Great place", UserScore: 4.5f);

        // POST /api/comment

        [Fact]
        public async Task PostComment_WhenSucceeds_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<CreateCommentCommand>(), default))
                   .ReturnsAsync(new Success("Comment added."));

            var result = await BuildController().PostComment(ValidForm());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PostComment_WhenMediatorFails_ReturnsBadRequest()
        {
            _sender.Setup(s => s.Send(It.IsAny<CreateCommentCommand>(), default))
                   .ReturnsAsync(new Fail("Invalid data."));

            var result = await BuildController().PostComment(ValidForm());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PostComment_WhenUserIdMissing_ReturnsUnauthorized()
        {
            var result = await BuildController(userId: null, username: "johndoe")
                .PostComment(ValidForm());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task PostComment_WhenUsernameMissing_ReturnsUnauthorized()
        {
            var result = await BuildController(userId: "user-001", username: null)
                .PostComment(ValidForm());

            Assert.IsType<UnauthorizedResult>(result);
        }

        // GET /api/comment/user-comments

        [Fact]
        public async Task GetUserComments_WhenAuthenticated_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetUserCommentsQuery>(), default))
                   .ReturnsAsync(new List<Core.Entities.Comment>());

            var result = await BuildController().GetUserComments(
                new GetUserCommentsRequest(null, null, null));

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUserComments_WhenUserIdMissing_ReturnsUnauthorized()
        {
            var result = await BuildController(userId: null)
                .GetUserComments(new GetUserCommentsRequest(null, null, null));

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
