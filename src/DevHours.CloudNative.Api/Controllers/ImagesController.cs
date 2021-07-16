using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DevHours.CloudNative.Repositories;
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
        private readonly IBlobRepository<string> repository;

        public ImagesController(ILogger<ImagesController> logger, IBlobRepository<string> repository)
            => (this.logger, this.repository) = (logger, repository);

        [HttpGet]
        public IAsyncEnumerable<string> GetImageNames(int roomId, CancellationToken token = default)
            => repository.ListNamesAsync($"{roomId}", token);

        [HttpGet("{id:guid}", Name = "GetImage")]
        public async Task<FileStreamResult> GetImageAsync(int roomId, Guid id, CancellationToken token = default)
        {
            var result = await repository.DownloadAsync($"{roomId}/{id}", token);

            return File(result.Content, result.ContentType, $"{id}.png");
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(int roomId, IFormFile file, CancellationToken token = default)
        {
            var id = Guid.NewGuid();
            var key = $"{roomId}/{id}";
            await repository.UploadAsync(key, file.OpenReadStream(), file.ContentType, token);
            return CreatedAtRoute("GetImage", new { roomId = roomId, id = id }, new { name = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int roomId, Guid id, CancellationToken token = default)
        {
            await repository.RemoveAsync($"{roomId}/{id}", token);            
            return NoContent();
        }
    }
}