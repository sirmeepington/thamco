
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThAmCo.Venues.Data;

namespace ThAmCo.VenuesFacade
{
    public interface IAvailabilities
    {
        Task<List<AvailabilityDto>> GetAvailabilities(string type, DateTime from, DateTime to);

    }
}
