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
        private readonly IDataRepository<Room> repository;

        public RoomsController(ILogger<RoomsController> logger, IDataRepository<Room> repository)
            => (this.logger, this.repository) = (logger, repository);

        [HttpGet]
        [EnableQuery]
        public IQueryable<Room> GetRooms() => repository.Query();

        [HttpGet("{id:int}", Name = "GetRoom")]
        public ValueTask<Room> GetRoomAsync(int id, CancellationToken token = default) 
            => repository.GetAsync(id, token);

        [HttpPost]
        public async Task<ActionResult<Room>> AddRoom(Room room, CancellationToken token = default)
        {
            var addedRoom = await repository.AddAsync(room, token);
            return CreatedAtRoute("GetRoom", new { id = addedRoom.Id }, addedRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Room room, CancellationToken token = default)
        {
            if (room.Id != id)
            {
                throw new Exception("Id mismatch");
            }

            await repository.UpdateAsync(room, token);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken token = default)
        {
            var stored = await repository.GetAsync(id, token);

            if (stored is null) 
            {
                throw new NullReferenceException("stored");
            }

            await repository.DeleteAsync(stored, token);

            return NoContent();
        }
    }
}