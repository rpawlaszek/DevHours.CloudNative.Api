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
        private readonly IDataRepository<Booking> repository;

        public BookingsController(ILogger<BookingsController> logger, IDataRepository<Booking> repository)
            => (this.logger, this.repository) = (logger, repository);

        [HttpGet]
        [EnableQuery]
        public IQueryable<Booking> GetBookings() => repository.Query();

        [HttpGet("{id}", Name = "GetBooking")]
        public ValueTask<Booking> GetBooking(int id, CancellationToken token = default) 
            => repository.GetAsync(id, token);

        [HttpPost]
        public async Task<ActionResult<Booking>> Book(Booking booking, CancellationToken token = default)
        {
            var addedBooking = await repository.AddAsync(booking, token);
            return CreatedAtRoute("GetBooking", new { id = addedBooking.Id }, addedBooking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id, CancellationToken token = default)
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