﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ThAmCo.VenuesFacade.Availabilities
{
    /// <summary>
    /// A Data Transfer Object that represents an <see cref="Venues.Data.Availability"/>.
    /// </summary>
    public class AvailabilityApiGetDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public DateTime Date { get; set; }

        public double CostPerHour { get; set; }
    }
}
