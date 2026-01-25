using Microsoft.Extensions.Options;
using System.IO.Compression;
using System.Security.Cryptography;
using TAE.UploadService.Infrastructure;
using TAE.UploadService.Models;
using Microsoft.Data.SqlClient;

namespace TAE.UploadService.Repositories
{
    public class UploadRepository
    {
        private readonly string _tempDir;
        private readonly string _finalDir;
        private readonly string _connectionString;

        public UploadRepository(IConfiguration config, IOptions<UploadStorageOptions> options)
        {
            _connectionString = config.GetConnectionString("UploadDb")
                ?? throw new InvalidOperationException("UploadDb connection string not found");

            var uploadConfig = options.Value;
            _tempDir = uploadConfig.TempDirectory;
            _finalDir = uploadConfig.FinalDirectory;

            EnsureDirectoryExists(_tempDir);
            EnsureDirectoryExists(_finalDir);
        }


        public async Task<Guid> StoreAsync(IFormFile file)
        {
            var uploadId = Guid.NewGuid();
            var tempPath = Path.Combine(_tempDir, $"{uploadId}.tmp");
            var finalPath = Path.Combine(_finalDir, $"{uploadId}.xlsx");

            try
            {
                string hash;

                await using (var source = file.OpenReadStream())
                await using (var target = File.Create(tempPath))
                using (var sha256 = SHA256.Create())
                await using (var crypto = new CryptoStream(target, sha256, CryptoStreamMode.Write))
                {
                    await source.CopyToAsync(crypto);
                    await crypto.FlushAsync();
                    hash = Convert.ToHexString(sha256.Hash!);
                }

                ValidateExcelStructure(tempPath);

                var upload = new UploadEntity
                {
                    UploadId = uploadId,
                    OriginalFileName = file.FileName,
                    FileHash = hash,
                    FileSize = file.Length,
                    StoragePath = finalPath,
                    Status = "STORED",
                    UploadedAt = DateTime.UtcNow
                };

                await StoreMetadataAsync(upload);

                File.Move(tempPath, finalPath);

                return uploadId;
            }
            catch
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);

                throw;
            }
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Directory path cannot be null or empty.", nameof(path));

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create directory: {path}", ex);
            }
        }

        private static void ValidateExcelStructure(string filePath)
        {
            try
            {
                using var archive = ZipFile.OpenRead(filePath);

                var hasWorkbook = archive.Entries
                    .Any(e => e.FullName.Equals("xl/workbook.xml", StringComparison.OrdinalIgnoreCase));

                var hasContentTypes = archive.Entries
                    .Any(e => e.FullName.Equals("[Content_Types].xml", StringComparison.OrdinalIgnoreCase));

                if (!hasWorkbook || !hasContentTypes)
                {
                    throw new InvalidDataException("File is not a valid Excel workbook.");
                }
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Failed to validate Excel structure.", ex);
            }
        }

        public async Task<UploadEntity> StoreMetadataAsync(UploadEntity entity)
        {
            const string sql = @"
        INSERT INTO Uploads (UploadId, OriginalFileName, FileHash, FileSize, StoragePath, Status, UploadedAt)
        VALUES (@UploadId, @OriginalFileName, @FileHash, @FileSize, @StoragePath, @Status, @UploadedAt)";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UploadId", entity.UploadId);
            command.Parameters.AddWithValue("@OriginalFileName", entity.OriginalFileName);
            command.Parameters.AddWithValue("@FileHash", entity.FileHash);
            command.Parameters.AddWithValue("@FileSize", entity.FileSize);
            command.Parameters.AddWithValue("@StoragePath", entity.StoragePath);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@UploadedAt", entity.UploadedAt);

            await command.ExecuteNonQueryAsync();

            return entity;
        }
    }
}
