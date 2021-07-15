using System;

namespace DevHours.CloudNative.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}