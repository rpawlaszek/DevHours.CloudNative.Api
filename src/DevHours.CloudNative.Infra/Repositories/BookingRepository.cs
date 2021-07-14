using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevHours.CloudNative.DataAccess;
using DevHours.CloudNative.Domain;

namespace DevHours.CloudNative.Repositories 
{
    public class BookingRepository : IDataRepository<Booking>
    {
        private readonly HotelContext context;

        public BookingRepository(HotelContext context) => this.context = context;

        public IQueryable<Booking> Query() => context.Bookings;

        public ValueTask<Booking> GetAsync(int id) => context.Bookings.FindAsync(id);

        public async Task<Booking> AddAsync(Booking room, CancellationToken token = default) 
        {
            context.Bookings.Add(room);
            await context.SaveChangesAsync(token);
            return room;
        }

        public async Task UpdateAsync(Booking room, CancellationToken token = default) 
        {
            context.Entry(room).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(Booking room, CancellationToken token = default) 
        {
            context.Bookings.Remove(room);
            await context.SaveChangesAsync(token);
        }
    }
}