using BookTheRoom.Application.DTO;
using BookTheRoom.Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace BookTheRoom.Application.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloundinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloundinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(UploadedFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Stream.Length > 0)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.Stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloundinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
        {
            var publicId = publicUrl.Split('/').Last().Split('.')[0];
            var deleteParams = new DeletionParams(publicId);
            return await _cloundinary.DestroyAsync(deleteParams);
        }
    }
}