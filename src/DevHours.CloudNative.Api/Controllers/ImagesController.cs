using DevHours.CloudNative.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DevHours.CloudNative.Api.Controllers
{
    [Route("api/rooms/{roomId:int}/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly RoomImagesService imagesService;

        public ImagesController(ILogger<ImagesController> logger, RoomImagesService imagesService)
            => (this.logger, this.imagesService) = (logger, imagesService);

        [HttpGet]
        [EnableQuery]
        public IAsyncEnumerable<string> GetImageNames(int roomId, CancellationToken token = default)
            => imagesService.ListNamesAsync(roomId, token);

        [HttpGet("{id:guid}", Name = "GetImage")]
        public async Task<FileStreamResult> GetImageAsync(int roomId, Guid id, CancellationToken token = default)
        {
            var (stream, type) = await imagesService.DownloadAsync(roomId, id, token);
            return File(stream, type);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(int roomId, IFormFile file, CancellationToken token = default)
        {
            var id = Guid.NewGuid();
            var name = await imagesService.UploadAsync(roomId, id, file.OpenReadStream(), file.ContentType, token);
            return CreatedAtRoute("GetImage", new { roomId = roomId, id = id }, new { name = name });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int roomId, Guid id, CancellationToken token = default)
        {
            await imagesService.RemoveAsync(roomId, id, token);
            return NoContent();
        }
    }
}