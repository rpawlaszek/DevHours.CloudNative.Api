using System.Collections.Generic;

namespace DevHours.CloudNative.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}