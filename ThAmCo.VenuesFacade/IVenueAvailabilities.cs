
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThAmCo.Venues.Data;

namespace ThAmCo.VenuesFacade
{
    public interface IVenueAvailabilities
    {
        Task<List<Availability>> GetAvailabilities(string type, DateTime from, DateTime to);

    }
}
