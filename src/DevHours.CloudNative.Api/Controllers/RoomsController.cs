using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevHours.CloudNative.Domain;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;
using DevHours.CloudNative.Api.Data.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevHours.CloudNative.Core.Services;
using System;

namespace DevHours.CloudNative.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly RoomService service;
        private readonly IMapper mapper;
        private readonly IConfigurationProvider configurationProvider;

        public RoomsController(ILogger<RoomsController> logger, RoomService service, IMapper mapper, IConfigurationProvider configurationProvider)
            => (this.logger, this.service, this.mapper, this.configurationProvider) = (logger, service, mapper, configurationProvider);

        [HttpGet]
        [EnableQuery]
        public IQueryable<RoomDto> GetRooms() => service.Query().ProjectTo<RoomDto>(configurationProvider);

        [HttpGet("{id:int}", Name = "GetRoom")]
        public async Task<ActionResult<RoomDto>> GetRoomAsync([FromODataUri] int id)
        {
            var room = await service.GetRoomAsync(id);
            return mapper.Map<RoomDto>(room);
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> AddRoom(RoomDto room, CancellationToken token = default)
        {
            var addedRoom = await service.AddRoomAsync(mapper.Map<Room>(room), token);
            return CreatedAtRoute("GetRoom", new { id = addedRoom.Id }, mapper.Map<RoomDto>(addedRoom));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoomDto room, CancellationToken token = default)
        {
            if (room.Id != id)
            {
                throw new Exception("Id mismatch");
            }

            await service.UpdateRoomAsync(mapper.Map<Room>(room), token);

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