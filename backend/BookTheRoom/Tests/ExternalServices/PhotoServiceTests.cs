using Application.ExternalServices;
using Application.Interfaces;
using Application.Settings;
using CloudinaryDotNet.Actions;

namespace Tests.ExternalServices
{
    public class PhotoServiceTests
    {
        private readonly Mock<IPhotoService> _options;

        public PhotoServiceTests()
        {
            var optionsMock = new Mock<IOptions<CloudinarySettings>>();

            optionsMock.Setup(x => x.Value).Returns(
                new CloudinarySettings
                {
                    CloudName = "your_cloud_name",
                    ApiKey = "your_api_key",
                    ApiSecret = "your_api_secret"
                });

            var photoService = new PhotoService(optionsMock.Object);
            _options = new Mock<IPhotoService>();
        }

        [Fact]
        public async Task AddPhotoAsync_Success()
        {
            var stream = new MemoryStream();
            var fileName = "test.jpg";


            var expectedResult = new ImageUploadResult
            {
                Url = new Uri("https://example.com/test.jpg"),
                PublicId = "some_id"
            };
            _options.Setup(service => service.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()))
                             .ReturnsAsync(expectedResult);

            var photoService = _options.Object;


            var result = await photoService.AddPhotoAsync(fileName, stream);


            Assert.NotNull(result);
            Assert.NotNull(result.Url);
            Assert.False(string.IsNullOrEmpty(result.Url.ToString()));
            Assert.False(string.IsNullOrEmpty(result.PublicId));
        }

        [Fact]
        public async Task DeletePhotoAsync_Success()
        {
            var publicUrl = "https://example.com/test.jpg";
            var expectedResult = new DeletionResult
            {
                Result = "Image deleted successfully.",
            };

            _options.Setup(service => service.DeletePhotoAsync(publicUrl))
                             .ReturnsAsync(expectedResult);

            var photoService = _options.Object;

            var result = await photoService.DeletePhotoAsync(publicUrl);

            Assert.NotNull(result);
            Assert.Equal("Image deleted successfully.", result.Result);
        }

    }
}
