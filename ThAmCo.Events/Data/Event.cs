using System;
using System.Collections.Generic;
using ThAmCo.Venues.Data;

namespace ThAmCo.Events.Data
{
    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        public string TypeId { get; set; }

        public List<GuestBooking> Bookings { get; set; }

        public string VenueReservation { get; set; }
    }
}
