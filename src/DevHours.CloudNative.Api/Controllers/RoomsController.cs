using DevHours.CloudNative.Core.Services;
using DevHours.CloudNative.Domain;
using DevHours.CloudNative.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
        private readonly RoomService service;

        public RoomsController(ILogger<RoomsController> logger, RoomService service)
            => (this.logger, this.service) = (logger, service);

        [HttpGet]
        [EnableQuery]
        public IQueryable<Room> GetRooms() => service.Query();

        [HttpGet("{id:int}", Name = "GetRoom")]
        public ValueTask<Room> GetRoomAsync(int id, CancellationToken token = default) 
            => service.GetRoomAsync(id, token);

        [HttpPost]
        public async Task<ActionResult<Room>> AddRoom(Room room, CancellationToken token = default)
        {
            var addedRoom = await service.AddRoomAsync(room, token);
            return CreatedAtRoute("GetRoom", new { id = addedRoom.Id }, addedRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Room room, CancellationToken token = default)
        {
            if (room.Id != id)
            {
                throw new Exception("Id mismatch");
            }

            await service.UpdateRoomAsync(room, token);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken token = default)
        {
            await service.DeleteRoomAsync(id, token);
            return NoContent();
        }
    }
}