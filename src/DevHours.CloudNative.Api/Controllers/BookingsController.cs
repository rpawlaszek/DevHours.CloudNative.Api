using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevHours.CloudNative.Domain;
using DevHours.CloudNative.Repositories;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.OData.Query;
using AutoMapper;
using DevHours.CloudNative.Api.Data.Dtos;
using AutoMapper.QueryableExtensions;

namespace DevHours.CloudNative.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IDataRepository<Booking> repository;
        private readonly IMapper mapper;
        private readonly IConfigurationProvider configurationProvider;

        public BookingsController(ILogger<BookingsController> logger, IDataRepository<Booking> repository, IMapper mapper, IConfigurationProvider configurationProvider)
            => (this.logger, this.repository, this.mapper, this.configurationProvider) = (logger, repository, mapper, configurationProvider);

        [HttpGet]
        [EnableQuery]
        public IQueryable<BookingDto> GetBookings() => repository.Query().ProjectTo<BookingDto>(configurationProvider);

        [HttpGet("{id}", Name = "GetBooking")]
        public async ValueTask<ActionResult<BookingDto>> Get(int id)
        {
            var booking = await repository.GetAsync(id);
            return mapper.Map<BookingDto>(booking);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Create(BookingDto booking, CancellationToken token = default)
        {
            var addedBooking = await repository.AddAsync(mapper.Map<Booking>(booking), token);

            return CreatedAtRoute("GetBooking", new { id = addedBooking.Id }, mapper.Map<BookingDto>(addedBooking));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BookingDto booking, CancellationToken token = default)
        {
            var stored = await repository.GetAsync(id);

            if (stored is null)
            {
                return NotFound();
            }

            await repository.UpdateAsync(mapper.Map<Booking>(booking), token);

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