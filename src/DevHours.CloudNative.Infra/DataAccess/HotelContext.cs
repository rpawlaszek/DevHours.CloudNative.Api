using DevHours.CloudNative.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevHours.CloudNative.DataAccess
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options)
        : base(options) { }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => base.OnModelCreating(modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly));
    }
}