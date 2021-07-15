using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevHours.CloudNative.Domain;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.OData.Query;
using AutoMapper;
using DevHours.CloudNative.Api.Data.Dtos;
using AutoMapper.QueryableExtensions;
using DevHours.CloudNative.Core.Services;

namespace DevHours.CloudNative.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly RoomBookingService service;
        private readonly IMapper mapper;
        private readonly IConfigurationProvider configurationProvider;

        public BookingsController(ILogger<BookingsController> logger, RoomBookingService service, IMapper mapper, IConfigurationProvider configurationProvider)
            => (this.logger, this.service, this.mapper, this.configurationProvider) = (logger, service, mapper, configurationProvider);

        [HttpGet]
        [EnableQuery]
        public IQueryable<BookingDto> GetBookings() => service.Query().ProjectTo<BookingDto>(configurationProvider);

        [HttpGet("{id}", Name = "GetBooking")]
        public async ValueTask<ActionResult<BookingDto>> GetBooking(int id, CancellationToken token = default)
        {
            var booking = await service.GetBookingAsync(id, token);
            return mapper.Map<BookingDto>(booking);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Book(BookingDto booking, CancellationToken token = default)
        {
            var addedBooking = await service.Book(mapper.Map<Booking>(booking), token);
            return CreatedAtRoute("GetBooking", new { id = addedBooking.Id }, mapper.Map<BookingDto>(addedBooking));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id, CancellationToken token = default)
        {
            await service.CancelBooking(id, token);
            return NoContent();
        }
    }
}