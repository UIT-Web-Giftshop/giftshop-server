using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3.Model;

namespace Infrastructure.Interfaces.Services
{
    public interface IAWSS3BucketService
    {
        Task<bool> UploadFileAsync(Stream stream, string key, string contentType);
        Task<ListVersionsResponse> ListFilesAsync();
        Task<Stream> GetFileAsync(string key);
        Task<bool> DeleteFileAsync(string key);
        Task<bool> DeleteMultipleFilesAsync(IEnumerable<string> keys);
    }
}