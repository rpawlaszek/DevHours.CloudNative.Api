using DevHours.CloudNative.Api.Dtos;
using Microsoft.OData.ModelBuilder;

namespace DevHours.CloudNative.Api.OData
{
    public class HotelModelBuilder : ODataConventionModelBuilder
    {
        public HotelModelBuilder()
            : base()
        {
            var rooms = EntitySet<RoomDto>("Room");
            rooms.EntityType.HasKey(r => r.Id);
            rooms.EntityType.HasMany(r => r.Bookings).CascadeOnDelete();

            var bookings = EntitySet<BookingDto>("Booking");
            bookings.EntityType.HasKey(b => b.Id);
        }
    }
}