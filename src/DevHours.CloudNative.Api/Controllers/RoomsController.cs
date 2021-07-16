using DevHours.CloudNative.DataAccess;
using DevHours.CloudNative.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevHours.CloudNative.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly HotelContext context;

        public RoomsController(ILogger<RoomsController> logger, HotelContext context)
            => (this.logger, this.context) = (logger, context);

        [HttpGet]
        public IQueryable<Room> GetRooms() => context.Rooms;

        [HttpGet("{id:int}", Name = "GetRoom")]
        public ValueTask<Room> GetRoomAsync(int id, CancellationToken token = default) => context.Rooms.FindAsync(id, token);

        [HttpPost]
        public async Task<ActionResult<Room>> AddRoom(Room room, CancellationToken token = default)
        {
            var addedRoom = await context.Rooms.AddAsync(room, token);
            await context.SaveChangesAsync(token);
            return CreatedAtRoute("GetRoom", new { id = addedRoom.Entity.Id }, addedRoom.Entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Room room, CancellationToken token = default)
        {
            if (room.Id != id)
            {
                throw new Exception("Id mismatch");
            }

            context.Rooms.Update(room);
            await context.SaveChangesAsync(token);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken token = default)
        {
            var stored = await context.Rooms.FindAsync(id, token);

            if (stored is null) 
            {
                throw new NullReferenceException("stored");
            }

            context.Rooms.Remove(stored);
            await context.SaveChangesAsync(token);

            return NoContent();
        }
    }
}