using System;
using ThAmCo.Venues.Data;

namespace ThAmCo.VenuesFacade
{
    public class AvailabilityDto
    {
        public DateTime Date { get; set; }

        public string VenueCode { get; set; }

        public Venue Venue { get; set; }

        public double CostPerHour { get; set; }

        public Reservation Reservation { get; set; }
    }
}