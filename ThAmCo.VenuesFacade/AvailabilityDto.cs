using System;
using ThAmCo.Venues.Data;

namespace ThAmCo.VenuesFacade
{
    public class AvailabilityDto
    {

        public string Code { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public DateTime Date { get; set; }

        public double CostPerHour { get; set; }

    }
}