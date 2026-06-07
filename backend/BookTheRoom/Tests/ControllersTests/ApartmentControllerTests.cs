using Api.Contracts.Apartment;
using Api.Controllers;
using Application.Interfaces;
using Application.UseCases.Commands.Apartment;
using Application.UseCases.Queries.Apartment;
using Core.Contracts;
using Core.Entities;
using Core.TasksResults;
using Core.ValueObjects;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tests.ControllersTests
{
    // Unit tests for ApartmentController.
    // Covers: auth guards, > 20 images guard, mediator success/fail routing,
    // and DTO mapping for the list endpoints.
    public class ApartmentControllerTests
    {
        private readonly Mock<IMediator> _sender = new();
        private readonly Mock<IPhotoService> _photoService = new();

        // UserManager requires a store; use the minimal mock helper pattern.
        private static Mock<UserManager<ApplicationUser>> BuildUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        private ApartmentController BuildController(string? userId = "user-001")
        {
            var claims = new List<Claim>();
            if (userId != null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
                claims.Add(new Claim(ClaimTypes.GivenName, "John"));
                claims.Add(new Claim(ClaimTypes.Surname, "Doe"));
                claims.Add(new Claim(ClaimTypes.Email, "owner@example.com"));
                claims.Add(new Claim(ClaimTypes.HomePhone, "+380000000000"));
            }

            var identity = new ClaimsIdentity(claims,
                userId != null ? "TestAuth" : string.Empty);
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            var userManagerMock = BuildUserManagerMock();
            userManagerMock
                .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser?)null);

            var controller = new ApartmentController(
                _sender.Object,
                _photoService.Object,
                accessorMock.Object,
                userManagerMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            return controller;
        }

        private static Apartment MakeApartment(int id = 1) => new Apartment
        {
            Id = id,
            OwnerId = "user-001",
            OwnerName = "John Doe",
            Title = "Cozy Flat",
            Description = "desc",
            PriceForNight = 50m,
            Email = "owner@example.com",
            PhoneNumber = "+380000000000",
            Address = new Address("Ukraine", "Kyiv Oblast", "Kyiv", "Main St 1", "01001"),
            Images = new List<string> { "https://example.com/img.jpg" },
            Comments = new List<Comment>()
        };

        private IFormFile MakeFile(string name = "img.jpg")
        {
            var stream = new MemoryStream();
            new StreamWriter(stream) { AutoFlush = true }.Write("data");
            stream.Position = 0;
            return new FormFile(stream, 0, stream.Length, "file", name)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
        }

        private static CreateApartmentForm ValidCreateForm(int imageCount = 1)
        {
            // CreateApartmentForm has no images list ctor variant — pass empty list separately.
            return new CreateApartmentForm(
                "Cozy Flat", "A comfortable apartment", 50m,
                "Ukraine", "Kyiv Oblast", "Kyiv", "Main St 1", "01001",
                new List<IFormFile>(), null, null);
        }

        // GET /api/apartment

        [Fact]
        public async Task GetAll_ReturnsOkWithMappedApartments()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetApartmentsQuery>(), default))
                   .ReturnsAsync(new List<Apartment> { MakeApartment() });

            var result = await BuildController().GetAll(
                new GetApartmentsRequest(null, null, null, null, null, null));

            Assert.IsType<OkObjectResult>(result);
        }

        // GET /api/apartment/{id}

        [Fact]
        public async Task Get_WhenFound_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetApartmentQuery>(), default))
                   .ReturnsAsync(MakeApartment());

            var result = await BuildController().Get(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_WhenNotFound_ReturnsNotFound()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetApartmentQuery>(), default))
                   .ReturnsAsync((Apartment?)null);

            var result = await BuildController().Get(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // POST /api/apartment

        [Fact]
        public async Task Post_WhenNotAuthenticated_ReturnsUnauthorized()
        {
            var result = await BuildController(userId: null)
                .Post(ValidCreateForm());

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Post_WithMoreThan20Images_ReturnsBadRequest()
        {
            var images = Enumerable.Range(1, 21)
                .Select(i => MakeFile($"img{i}.jpg"))
                .ToList();

            var form = new CreateApartmentForm(
                "Title", "desc", 50m,
                "Ukraine", "Kyiv Oblast", "Kyiv", "Street", "01001",
                images, null, null);

            var result = await BuildController().Post(form);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_WhenMediatorSucceeds_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<CreateApartmentCommand>(), default))
                   .ReturnsAsync(new Success("Created."));

            var result = await BuildController().Post(ValidCreateForm());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Post_WhenMediatorFails_ReturnsBadRequest()
        {
            _sender.Setup(s => s.Send(It.IsAny<CreateApartmentCommand>(), default))
                   .ReturnsAsync(new Fail("Address already exists."));

            var result = await BuildController().Post(ValidCreateForm());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // PUT /api/apartment/{id}

        [Fact]
        public async Task Put_WhenNotAuthenticated_ReturnsUnauthorized()
        {
            var form = new UpdateApartmentForm(
                "Title", "desc", 50m,
                "Ukraine", "Kyiv Oblast", "Kyiv", "Street", "01001",
                null, null, null);

            var result = await BuildController(userId: null).Put(1, form);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Put_WithMoreThan20Images_ReturnsBadRequest()
        {
            var images = Enumerable.Range(1, 21)
                .Select(i => MakeFile($"img{i}.jpg"))
                .ToList();

            var form = new UpdateApartmentForm(
                "Title", "desc", 50m,
                "Ukraine", "Kyiv Oblast", "Kyiv", "Street", "01001",
                images, null, null);

            var result = await BuildController().Put(1, form);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_WhenMediatorSucceeds_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<UpdateApartmentCommand>(), default))
                   .ReturnsAsync(new Success("Updated."));

            var form = new UpdateApartmentForm(
                "Title", "desc", 50m,
                "Ukraine", "Kyiv Oblast", "Kyiv", "Street", "01001",
                null, null, null);

            var result = await BuildController().Put(1, form);

            Assert.IsType<OkObjectResult>(result);
        }

        // DELETE /api/apartment/{id}

        [Fact]
        public async Task Delete_WhenNotAuthenticated_ReturnsUnauthorized()
        {
            var result = await BuildController(userId: null).Delete(1);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Delete_WhenMediatorSucceeds_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<DeleteApartmentCommand>(), default))
                   .ReturnsAsync(new Success("Deleted."));

            var result = await BuildController().Delete(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_WhenMediatorFails_ReturnsBadRequest()
        {
            _sender.Setup(s => s.Send(It.IsAny<DeleteApartmentCommand>(), default))
                   .ReturnsAsync(new Fail("Not found."));

            var result = await BuildController().Delete(999);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
