using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Domain.Settings;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class AWSS3BucketService : IAWSS3BucketService
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly AWSS3Settings _awsS3Settings;
        private readonly ILogger<AWSS3BucketService> _logger;

        public AWSS3BucketService(
            IAmazonS3 amazonS3, 
            IOptions<AWSS3Settings> awsS3Settings, 
            ILogger<AWSS3BucketService> logger)
        {
            _amazonS3 = amazonS3;
            _logger = logger;
            _awsS3Settings = awsS3Settings.Value;
        }

        public async Task<bool> UploadFileAsync(Stream stream, string key, string contentType)
        {
            try
            {
                var putRequest = new PutObjectRequest()
                {
                    InputStream = stream,
                    BucketName = _awsS3Settings.BucketName,
                    Key = key,
                    ContentType = contentType
                };
                var response = await _amazonS3.PutObjectAsync(putRequest);
                return response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new Exception(e.Message);
            }
        }

        public async Task<ListVersionsResponse> ListFilesAsync()
        {
            return await _amazonS3.ListVersionsAsync(_awsS3Settings.BucketName);
        }

        public async Task<Stream> GetFileAsync(string key)
        {
            var getRequest = new GetObjectRequest()
            {
                BucketName = _awsS3Settings.BucketName,
                Key = key
            };
            var response = await _amazonS3.GetObjectAsync(getRequest);
            return response.HttpStatusCode == HttpStatusCode.OK 
                ? response.ResponseStream 
                : null;
        }

        public async Task<bool> DeleteFileAsync(string key)
        {
            try
            {
                var deleteRequest = new DeleteObjectRequest()
                {
                    BucketName = _awsS3Settings.BucketName,
                    Key = key
                };
                var response = await _amazonS3.DeleteObjectAsync(deleteRequest);
                return response.HttpStatusCode == HttpStatusCode.NoContent;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteMultipleFilesAsync(IEnumerable<string> keys)
        {
            try
            {
                var deleteRequest = new DeleteObjectsRequest() { BucketName = _awsS3Settings.BucketName, };
             
                using var iter = keys.GetEnumerator();
                while (iter.MoveNext())
                {
                    deleteRequest.AddKey(iter.Current);    
                }

                var response = await _amazonS3.DeleteObjectsAsync(deleteRequest);
                return response.HttpStatusCode == HttpStatusCode.NoContent;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new Exception(e.Message);
            }
        }
    }
}