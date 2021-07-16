using System;
using System.Collections.Generic;

namespace DevHours.CloudNative.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}