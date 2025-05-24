using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Commands.Hotel;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Api.Controllers;
using Api.Contracts.Hotel;
using Core.TasksResults;

namespace Tests.ControllersTests
{  
    public class HotelControllerTests
    {
        private readonly Mock<IPhotoService> _mockPhotoService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly HotelController _controller;

        public HotelControllerTests()
        {
            _mockPhotoService = new Mock<IPhotoService>();
            _mockMediator = new Mock<IMediator>();
            _controller = new HotelController(_mockMediator.Object, _mockPhotoService.Object);
        }
        [Fact]
        public async Task Put_WithInvalidData_ReturnsBadRequest()
        {
            int hotelId = 1;
            var form = new UpdateHotelForm
            (
                "Updated Hotel",
                "Updated Description",
                -4,
                true,
                "Country",
                "State",
                "City",
                "Street",
                "PostalCode",           
                new List<IFormFile>
                {
                    CreateMockFormFile("test1.jpg", "image/jpeg"),
                    CreateMockFormFile("test2.jpg", "image/jpeg"),
                    CreateMockFormFile("test3.jpg", "image/jpeg"),
                    CreateMockFormFile("test4.jpg", "image/jpeg"),
                    CreateMockFormFile("test5.jpg", "image/jpeg"),
                    CreateMockFormFile("test6.jpg", "image/jpeg"),
                    CreateMockFormFile("test7.jpg", "image/jpeg"),
                    CreateMockFormFile("test8.jpg", "image/jpeg"),
                    CreateMockFormFile("test9.jpg", "image/jpeg"),
                    CreateMockFormFile("test10.jpg", "image/jpeg"),
                    CreateMockFormFile("test11.jpg", "image/jpeg"),
                    CreateMockFormFile("test12.jpg", "image/jpeg"),
                    CreateMockFormFile("test13.jpg", "image/jpeg"),
                    CreateMockFormFile("test14.jpg", "image/jpeg"),
                    CreateMockFormFile("test15.jpg", "image/jpeg"),
                    CreateMockFormFile("test16.jpg", "image/jpeg"),
                    CreateMockFormFile("test17.jpg", "image/jpeg"),
                    CreateMockFormFile("test18.jpg", "image/jpeg"),
                    CreateMockFormFile("test19.jpg", "image/jpeg"),
                    CreateMockFormFile("test20.jpg", "image/jpeg"),
                    CreateMockFormFile("test21.jpg", "image/jpeg")
                }
            );

            var uploadedImageUrls = new List<string>
            {
                "http://image1.com",
                "http://image2.com",
                "http://image3.com",
                "http://image4.com",
                "http://image5.com",
                "http://image6.com",
                "http://image7.com",
                "http://image8.com",
                "http://image9.com",
                "http://image10.com",
                "http://image11.com",
                "http://image12.com",
                "http://image13.com",
                "http://image14.com",
                "http://image15.com",
                "http://image16.com",
                "http://image17.com",
                "http://image18.com",
                "http://image19.com",
                "http://image20.com",
                "http://image21.com"
            };

            _mockPhotoService
                .SetupSequence(x => x.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()))
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[0]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[1]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[2]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[3]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[4]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[5]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[6]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[7]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[8]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[9]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[10]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[11]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[12]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[13]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[14]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[15]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[16]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[17]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[18]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[19]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[20]) });

            _mockMediator
                .Setup(x => x.Send(It.IsAny<UpdateHotelCommand>(), default))
                .ReturnsAsync(new Fail("Validation is failed.", Core.Enums.ErrorStatuses.ValidationError));

            var result = await _controller.Put(hotelId, form);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var actualResult = badRequestResult.Value as Fail;

            Assert.Equal("Validation is failed.", actualResult?.Message);
            Assert.Equal(false, actualResult?.IsSuccess);
            Assert.Equal(Core.Enums.ErrorStatuses.ValidationError, actualResult?.Status);
            _mockPhotoService.Verify(x => x.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()), Times.Exactly(21));
            _mockMediator.Verify(x => x.Send(It.IsAny<UpdateHotelCommand>(), default), Times.Once);
        }
        [Fact]
        public async Task Put_WithValidData_ReturnsOk()
        {
            int hotelId = 1;
            var form = new UpdateHotelForm
            (
                "Updated Hotel",
                "Updated Description",
                4,
                true,
                "Country",
                "State",
                "City",
                "Street",
                "PostalCode",
                new List<IFormFile>
                {
                    CreateMockFormFile("test1.jpg", "image/jpeg"),
                    CreateMockFormFile("test2.jpg", "image/jpeg")
                }
            );

            var uploadedImageUrls = new List<string> { "http://image1.com", "http://image2.com" };

            _mockPhotoService
                .SetupSequence(x => x.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()))
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[0]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[1]) });

            _mockMediator
                .Setup(x => x.Send(It.IsAny<UpdateHotelCommand>(), default))
                .ReturnsAsync(new Success("Entity 'Hotel' was updated successfully."));

            var result = await _controller.Put(hotelId, form);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = okResult.Value as Success;
            Assert.Equal("Entity 'Hotel' was updated successfully.", actualResult?.Message);
            Assert.Equal(true, actualResult?.IsSuccess);
            _mockPhotoService.Verify(x => x.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()), Times.Exactly(2));
            _mockMediator.Verify(x => x.Send(It.IsAny<UpdateHotelCommand>(), default), Times.Once);
        }

        private IFormFile CreateMockFormFile(string fileName, string contentType)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Fake image content");
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }

}
