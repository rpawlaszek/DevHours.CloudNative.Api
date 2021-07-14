using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevHours.CloudNative.Domain;
using DevHours.CloudNative.Repositories;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;
using DevHours.CloudNative.Api.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace DevHours.CloudNative.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IDataRepository<Room> repository;
        private readonly IMapper mapper;
        private readonly IConfigurationProvider configurationProvider;

        public RoomsController(ILogger<RoomsController> logger, IDataRepository<Room> repository, IMapper mapper, IConfigurationProvider configurationProvider)
            => (this.logger, this.repository, this.mapper, this.configurationProvider) = (logger, repository, mapper, configurationProvider);

        [HttpGet]
        [EnableQuery]
        public IQueryable<RoomDto> GetRooms() => repository.Query().ProjectTo<RoomDto>(configurationProvider);

        [HttpGet("{id:int}", Name = "GetRoom")]
        public async Task<ActionResult<RoomDto>> GetRoomAsync([FromODataUri] int id)
        {
            var room = await repository.GetAsync(id);
            return mapper.Map<RoomDto>(room);
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> Create(RoomDto room, CancellationToken token = default)
        {
            var addedRoom = await repository.AddAsync(mapper.Map<Room>(room), token);

            return CreatedAtRoute("GetRoom", new { id = addedRoom.Id }, mapper.Map<RoomDto>(addedRoom));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoomDto room, CancellationToken token = default)
        {
            var stored = await repository.GetAsync(id);

            if (stored is null)
            {
                return NotFound();
            }

            await repository.UpdateAsync(mapper.Map<Room>(room), token);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken token = default)
        {
            var stored = await repository.GetAsync(id);

            if (stored is null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(stored, token);

            return NoContent();
        }
    }
}