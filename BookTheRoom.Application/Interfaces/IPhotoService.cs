using CloudinaryDotNet.Actions;

namespace BookTheRoom.Application.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(string fileName, Stream stream);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
