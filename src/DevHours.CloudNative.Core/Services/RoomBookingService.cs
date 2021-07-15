using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevHours.CloudNative.Domain;
using DevHours.CloudNative.Repositories;
using Microsoft.Extensions.Logging;

namespace DevHours.CloudNative.Core.Services
{
    public class RoomBookingService
    {
        private readonly ILogger logger;
        private readonly IDataRepository<Booking> bookingsRepository;

        private readonly IDataRepository<Room> roomsRepository;

        public RoomBookingService(ILogger<RoomBookingService> logger, IDataRepository<Booking> bookingsRepository, IDataRepository<Room> roomsRepository)
            => (this.logger, this.bookingsRepository, this.roomsRepository) = (logger, bookingsRepository, roomsRepository);

        public IQueryable<Booking> Query() => bookingsRepository.Query();

        public ValueTask<Booking> GetBookingAsync(int bookingId, CancellationToken token = default) => bookingsRepository.GetAsync(bookingId, token);
          
        public async Task<Booking> Book(Booking bookingRequest, CancellationToken token = default)
        {
            var room = await roomsRepository.GetAsync(bookingRequest.RoomId, token);

            if (room is null) 
            {
                throw new NullReferenceException("There is no such room");
            }
            
            if (bookingRequest.EndDate < bookingRequest.StartDate)
            {
                throw new Exception("Start and End dates must be in correct order");
            }

            if (bookingRequest.StartDate < DateTime.UtcNow.Date)
            {
                throw new Exception("Booking for passed days is not possible");
            }

            var r = bookingRequest;

            if (bookingsRepository.Query()
                .Where(b => b.RoomId == bookingRequest.RoomId)
                .Where(b => 
                    (r.StartDate <= b.StartDate && r.EndDate >= b.StartDate) 
                    ||
                    (r.StartDate >= b.StartDate && r.EndDate <= b.EndDate)
                    ||
                    (r.StartDate <= b.EndDate && r.EndDate >= b.EndDate))
                .Any())
                {
                    throw new Exception("Room already booked at the specified range");
                }

            var createdBooking = await bookingsRepository.AddAsync(bookingRequest, token);

            return createdBooking; 
        }

        public async Task CancelBooking(int id, CancellationToken token = default)
        {            
            var stored = await bookingsRepository.GetAsync(id);

            if (stored is null)
            {
                throw new NullReferenceException("No such booking");
            }

            await bookingsRepository.DeleteAsync(stored, token);
        }
    }
}