namespace TAE.UploadService.Infrastructure
{
    public class UploadStorageOptions
    {
        public string TempDirectory { get; set; } = null!;
        public string FinalDirectory { get; set; } = null!;
    }
}