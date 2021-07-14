using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevHours.CloudNative.Models;
using DevHours.CloudNative.Repositories;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;

namespace DevHours.CloudNative.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IDataRepository<Room> roomsService;

        public RoomsController(ILogger<RoomsController> logger, IDataRepository<Room> roomsService)
            => (this.logger, this.roomsService) = (logger, roomsService);

        [HttpGet]
        [EnableQuery]
        public IQueryable<Room> GetRooms() => roomsService.Query();

        [HttpGet("{id:int}", Name = "GetRoom")]
        public async Task<ActionResult<Room>> GetRoomAsync([FromODataUri] int id) => await roomsService.GetAsync(id);

        [HttpPost]
        public async Task<ActionResult<Room>> Create(Room room, CancellationToken token = default) 
        {
            await roomsService.AddAsync(room, token);

            return CreatedAtRoute("GetRoom", new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Room room, CancellationToken token = default) 
        {
            var stored = await roomsService.GetAsync(id);

            if (stored is null)
            {
                return NotFound();
            }

            await roomsService.UpdateAsync(room, token);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken token = default) 
        {
            var stored = await roomsService.GetAsync(id);

            if (stored is null)
            {
                return NotFound();
            }

            await roomsService.DeleteAsync(stored, token);

            return NoContent();   
        }
    }
}