using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace DevHours.CloudNative.Repositories
{
    public class RoomImagesRepository : IBlobRepository<string>
    {
        private readonly BlobContainerClient client;
        
        public RoomImagesRepository(string connectionString, string containerName)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            client = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<Stream> DownloadAsync(string key, CancellationToken token = default)
        {
            await client.CreateIfNotExistsAsync(cancellationToken: token);
            var blobClient = client.GetBlobClient(key);

            var result = await blobClient.DownloadStreamingAsync(cancellationToken: token);

            return result.Value.Content;
        }

        public async IAsyncEnumerable<string> ListNamesAsync(string key, [EnumeratorCancellation] CancellationToken token = default)
        {
            await client.CreateIfNotExistsAsync(cancellationToken: token);

            var prefix = key.Split('/', StringSplitOptions.RemoveEmptyEntries).First();
            
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

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            await client.CreateIfNotExistsAsync(cancellationToken: token);
            var blobClient = client.GetBlobClient(key);

            await blobClient.DeleteAsync(cancellationToken: token);
        }

        public async Task UploadAsync(string key, Stream contents, string type, CancellationToken token = default)
        {
            await client.CreateIfNotExistsAsync(cancellationToken: token);
            var blobClient = client.GetBlobClient(key);
            await blobClient.UploadAsync(contents, token);
        }
    }
}