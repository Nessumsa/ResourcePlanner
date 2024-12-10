using ResourcePlanner.Interfaces.Adapters;

namespace ResourcePlanner.UseCases
{
    /// <summary>
    /// A handler class for use cases related to images, 
    /// providing methods for uploading and deleting images.
    /// </summary>
    public class ImageHandler
    {
        private readonly IImageAdapter _imageAdapter;

        /// <summary>
        /// Initializes a new instance of the ImageHandler class with the specified image adapter.
        /// </summary>
        /// <param name="imageAdapter">The adapter used for interacting with the backend API for image-related operations.</param>
        public ImageHandler(IImageAdapter imageAdapter)
        {
            this._imageAdapter = imageAdapter;
        }

        /// <summary>
        /// Uploads an image file to the backend using the provided file path.
        /// </summary>
        /// <param name="filePath">The path to the image file to upload.</param>
        /// <returns>The URL or identifier of the uploaded image, or null if the upload fails.</returns>
        public async Task<string?> UploadImage(string filePath)
        {
            return await _imageAdapter.UploadAsync(filePath);
        }

        /// <summary>
        /// Deletes an image from the backend using the provided file path.
        /// </summary>
        /// <param name="filePath">The path to the image file to delete.</param>
        /// <returns>True if the image was successfully deleted, otherwise false.</returns>
        public async Task<bool> DeleteImage(string filePath)
        {
            return await _imageAdapter.DeleteAsync(filePath);
        }
    }
}