using CloudinaryDotNet.Actions;

namespace Application.Interfaces
{
    /// <summary>
    /// Service for handling image upload and deletion via Cloudinary.
    /// </summary>
    public interface IPhotoService
    {
        /// <summary>
        /// Uploads an image to Cloudinary.
        /// </summary>
        /// <param name="fileName">Name of the file to be uploaded.</param>
        /// <param name="stream">Stream containing the image data.</param>
        /// <returns>Result of the image upload.</returns>
        Task<ImageUploadResult> AddPhotoAsync(string fileName, Stream stream);

        /// <summary>
        /// Deletes an image from Cloudinary.
        /// </summary>
        /// <param name="publicId">Public ID of the image to delete.</param>
        /// <returns>Result of the deletion operation.</returns>
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
