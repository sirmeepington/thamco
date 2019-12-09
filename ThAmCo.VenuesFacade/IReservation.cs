using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Venues.Models;

namespace ThAmCo.VenuesFacade
{
    public interface IReservation
    {

        Task<ReservationGetDto> GetReservation(string venueCode, DateTime startDate);
        
    }
}
