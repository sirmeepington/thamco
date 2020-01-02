
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThAmCo.Venues.Data;
using ThAmCo.VenuesFacade.Availabilities;

namespace ThAmCo.VenuesFacade
{
    /// <summary>
    /// Methods for interacting with the Availabilities endpoint of the Venues API.
    /// </summary>
    public interface IVenueAvailabilities
    {
        /// <summary>
        /// Gets a <see cref="AvailabilityApiGetDto"/> for every availability matching the 
        /// <paramref name="eventType"/> in a date between <paramref name="from"/> and <paramref name="to"/>.
        /// </summary>
        /// <param name="eventType">The <see cref="Venues.Data.EventType"/>'s Id.</param>
        /// <param name="from">The first date to filter for availabilities.</param>
        /// <param name="to">The last date to filder for availabiltiies.</param>
        /// <returns></returns>
        Task<List<AvailabilityApiGetDto>> GetAvailabilities(string eventType, DateTime from, DateTime to);

    }
}
