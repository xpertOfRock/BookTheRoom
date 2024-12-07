using Application.Interfaces;
using Application.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace Application.ExternalServices
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(string fileName, Stream stream)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult;
        }


        public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
        {
            var publicId = publicUrl
                .Split('/')
                .Last()
                .Split('.')[0];
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
