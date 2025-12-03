namespace TAE.UploadService.Repositories
{
    public class UploadRepository
    {
        public UploadRepository() { }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            // Implement file upload logic here
            return "File uploaded successfully.";
        }
    }
}
