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
    public class BookingsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly HotelContext context;

        public BookingsController(ILogger<BookingsController> logger, HotelContext context)
            => (this.logger, this.context) = (logger, context);

        [HttpGet]
        public IQueryable<Booking> GetBookings() => context.Bookings;

        [HttpGet("{id}", Name = "GetBooking")]
        public ValueTask<Booking> GetBooking(int id, CancellationToken token = default) 
            => context.Bookings.FindAsync(id);

        [HttpPost]
        public async Task<ActionResult<Booking>> Book(Booking booking, CancellationToken token = default)
        {
            var addedBooking = await context.Bookings.AddAsync(booking, token);
            await context.SaveChangesAsync(token);
            return CreatedAtRoute("GetBooking", new { id = addedBooking.Entity.Id }, addedBooking.Entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id, CancellationToken token = default)
        {
            var stored = await context.Bookings.FindAsync(id, token);

            if (stored is null) 
            {
                throw new NullReferenceException("stored");
            }

            context.Bookings.Remove(stored);
            await context.SaveChangesAsync(token);

            return NoContent();
        }
    }
}