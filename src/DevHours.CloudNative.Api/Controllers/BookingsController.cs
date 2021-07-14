using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevHours.CloudNative.Models;
using DevHours.CloudNative.Repositories;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.OData.Query;

namespace DevHours.CloudNative.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IDataRepository<Booking> service;

        public BookingsController(ILogger<BookingsController> logger, IDataRepository<Booking> service)
            => (this.logger, this.service) = (logger, service);

        [HttpGet]
        [EnableQuery]
        public IQueryable<Booking> GetBookings() => service.Query();

        [HttpGet]
        [Route("/api/rooms/{roomId}/bookings")]
        [EnableQuery]
        public IQueryable<Booking> GetRoomBookings(int roomId) => service.Query().Where(b => b.RoomId == roomId);

        [HttpGet("{id}", Name = "GetBooking")]
        public async ValueTask<ActionResult<Booking>> Get(int id) => await service.GetAsync(id);

        [HttpPost]
        public async Task<ActionResult<Booking>> Create(Booking room, CancellationToken token = default)
        {
            await service.AddAsync(room, token);

            return CreatedAtRoute("GetBooking", new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Booking room, CancellationToken token = default)
        {
            var stored = await service.GetAsync(id);

            if (stored is null)
            {
                return NotFound();
            }

            await service.UpdateAsync(room, token);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken token = default)
        {
            var stored = await service.GetAsync(id);

            if (stored is null)
            {
                return NotFound();
            }

            await service.DeleteAsync(stored, token);

            return NoContent();
        }
    }
}