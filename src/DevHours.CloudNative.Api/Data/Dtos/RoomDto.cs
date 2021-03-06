using System.Collections.Generic;

namespace DevHours.CloudNative.Api.Data.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<BookingDto> Bookings { get; set; }
    }
}