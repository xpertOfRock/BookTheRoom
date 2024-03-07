using BookTheRoom.Application.DTO;
using CloudinaryDotNet.Actions;

namespace BookTheRoom.Application.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(UploadedFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
