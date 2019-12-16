using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Venues.Models;

namespace ThAmCo.VenuesFacade
{
    public interface IVenueReservation
    {

        Task<ReservationGetDto> GetReservation(string venueCode, DateTime startDate);

        Task<ReservationGetDto> GetReservation(string reference);

        Task<bool> CancelReservation(string reference);

        Task<ReservationGetDto> CreateReservation(DateTime date, string venue);
        
    }
}
