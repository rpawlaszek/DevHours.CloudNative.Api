using System;

namespace DevHours.CloudNative.Api.Data.Dtos
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int RoomId { get; set; }
        public virtual RoomDto Room { get; set; }
    }
}