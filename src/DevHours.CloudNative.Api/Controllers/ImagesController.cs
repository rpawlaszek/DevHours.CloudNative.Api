using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DevHours.CloudNative.Api.Controllers
{
    [Route("api/rooms/{roomId:int}/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly BlobContainerClient client;

        public ImagesController(ILogger<ImagesController> logger, BlobContainerClient client)
            => (this.logger, this.client) = (logger, client);

        [HttpGet]
        public async IAsyncEnumerable<string> GetImageNames(int roomId, [EnumeratorCancellation] CancellationToken token = default)
        {            
            await client.CreateIfNotExistsAsync(cancellationToken: token);

            var prefix = $"{roomId}";

            var result = client.GetBlobsAsync(prefix: prefix, cancellationToken: token);

            await foreach (var page in result.AsPages())
            {
                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                foreach (var item in page.Values)
                {
                    var name = item.Name.Split('/', StringSplitOptions.RemoveEmptyEntries).Skip(1).SingleOrDefault();
                    yield return name;
                }
            }
        }

        [HttpGet("{id:guid}", Name = "GetImage")]
        public async Task<FileStreamResult> GetImageAsync(int roomId, Guid id, CancellationToken token = default)
        {
            await client.CreateIfNotExistsAsync(cancellationToken: token);
            var blobClient = client.GetBlobClient($"{roomId}/{id}");

            var result = await blobClient.DownloadStreamingAsync(cancellationToken: token);

            return File(result.Value.Content, result.Value.Details.ContentType, $"{id}.png");
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(int roomId, IFormFile file, CancellationToken token = default)
        {
            var id = Guid.NewGuid();
            var key = $"{roomId}/{id}";
            await client.CreateIfNotExistsAsync(cancellationToken: token);
            var blobClient = client.GetBlobClient(key);
            await blobClient.UploadAsync(file.OpenReadStream(), token);

            
            var properties = await blobClient.GetPropertiesAsync();

            var headers = new BlobHttpHeaders
            {
                // Set the MIME ContentType every time the properties 
                // are updated or the field will be cleared
                ContentType = file.ContentType,

                // Populate remaining headers with 
                // the pre-existing properties
                CacheControl = properties.Value.CacheControl,
                ContentDisposition = properties.Value.ContentDisposition,
                ContentEncoding = properties.Value.ContentEncoding,
                ContentHash = properties.Value.ContentHash
            };

            // Set the blob's properties.
            await blobClient.SetHttpHeadersAsync(headers);

            return CreatedAtRoute("GetImage", new { roomId = roomId, id = id }, new { name = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int roomId, Guid id, CancellationToken token = default)
        {
            await client.CreateIfNotExistsAsync(cancellationToken: token);
            var key = $"{roomId}/{id}";
            var blobClient = client.GetBlobClient(key);

            await blobClient.DeleteAsync(cancellationToken: token);
            
            return NoContent();
        }
    }
}