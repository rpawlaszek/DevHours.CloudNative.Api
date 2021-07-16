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
    public class BookingsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly RoomBookingService service;

        public BookingsController(ILogger<BookingsController> logger, RoomBookingService service)
            => (this.logger, this.service) = (logger, service);

        [HttpGet]
        [EnableQuery]
        public IQueryable<Booking> GetBookings() => service.Query();

        [HttpGet("{id}", Name = "GetBooking")]
        public ValueTask<Booking> GetBooking(int id, CancellationToken token = default) 
            => service.GetBookingAsync(id, token);

        [HttpPost]
        public async Task<ActionResult<Booking>> Book(Booking booking, CancellationToken token = default)
        {
            var addedBooking = await service.Book(booking, token);
            return CreatedAtRoute("GetBooking", new { id = addedBooking.Id }, addedBooking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id, CancellationToken token = default)
        {
            await service.CancelBooking(id, token);
            return NoContent();
        }
    }
}