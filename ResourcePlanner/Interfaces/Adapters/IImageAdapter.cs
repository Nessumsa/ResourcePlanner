namespace ResourcePlanner.Interfaces.Adapters
{
    public interface IImageAdapter
    {
        Task<string?> UploadAsync(string filePath);
        Task<bool> DeleteAsync(string filePath);
    }
}
