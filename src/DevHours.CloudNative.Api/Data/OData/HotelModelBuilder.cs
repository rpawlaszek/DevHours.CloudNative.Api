using DevHours.CloudNative.Domain;
using Microsoft.OData.ModelBuilder;

namespace DevHours.CloudNative.Api.Data.OData
{
    public class HotelModelBuilder : ODataConventionModelBuilder
    {
        public HotelModelBuilder()
            : base()
        {
            var rooms = EntitySet<Room>("Room");
            rooms.EntityType.HasKey(r => r.Id);
            rooms.EntityType.HasMany(r => r.Bookings).CascadeOnDelete();

            var bookings = EntitySet<Booking>("Booking");
            bookings.EntityType.HasKey(b => b.Id);
        }
    }
}