
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThAmCo.Venues.Data;
using ThAmCo.VenuesFacade.Availabilities;

namespace ThAmCo.VenuesFacade
{
    public interface IVenueAvailabilities
    {
        Task<List<AvailabilityApiGetDto>> GetAvailabilities(string type, DateTime from, DateTime to);

    }
}
