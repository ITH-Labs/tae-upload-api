namespace TAE.UploadService.Models
{
    public class UploadEntity
    {
        public Guid UploadId { get; set; }
        public string OriginalFileName { get; set; } = null!;
        public string FileHash { get; set; } = null!;
        public long FileSize { get; set; }
        public string StoragePath { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime UploadedAt { get; set; }
    }
}
