using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevHours.CloudNative.DataAccess;
using DevHours.CloudNative.Models;

namespace DevHours.CloudNative.Repositories 
{
    public class RoomsRepository  : IDataRepository<Room>
    {
        private readonly HotelContext context;

        public RoomsRepository(HotelContext context) => this.context = context;

        public IQueryable<Room> Query() => context.Rooms;

        public ValueTask<Room> GetAsync(int id) => context.Rooms.FindAsync(id);

        public async Task<Room> AddAsync(Room room, CancellationToken token = default) 
        {
            context.Rooms.Add(room);
            await context.SaveChangesAsync(token);
            return room;
        }

        public async Task UpdateAsync(Room room, CancellationToken token = default) 
        {
            context.Entry(room).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(Room room, CancellationToken token = default) 
        {
            context.Rooms.Remove(room);
            await context.SaveChangesAsync(token);
        }
    }
}