using ResourcePlanner.Interfaces.Adapters;

namespace ResourcePlanner.UseCases
{
    public class ImageHandler
    {
        private readonly IImageAdapter _imageAdapter;
        public ImageHandler(IImageAdapter imageAdapter)
        {
            this._imageAdapter = imageAdapter;
        }

        public async Task<string?> UploadImage(string filePath)
        {
            return await _imageAdapter.UploadAsync(filePath);
        }

        public async Task<bool> DeleteImage(string filePath)
        {
            return await _imageAdapter.DeleteAsync(filePath);
        }
    }
}
