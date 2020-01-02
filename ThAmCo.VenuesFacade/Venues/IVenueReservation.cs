using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Venues.Models;

namespace ThAmCo.VenuesFacade
{
    /// <summary>
    /// Methods responsible for getting, creating and cancelling reservations for events via
    /// a reference code.
    /// </summary>
    public interface IVenueReservation
    {
        /// <summary>
        /// Gets a reservation from the <see cref="Venues.Data.Venue.Code"/> and
        /// Event's <paramref name="startDate"/>.
        /// </summary>
        /// <param name="venueCode">The venue code of the Venue.</param>
        /// <param name="startDate">The start date of the event.</param>
        /// <returns>A <see cref="ReservationGetDto"/> describing the details of the 
        /// reservation if it exists; or a new/blank one if it doesn't.</returns>
        Task<ReservationGetDto> GetReservation(string venueCode, DateTime startDate);

        /// <summary>
        /// Gets a reservation via its reference code. The reference code is a string 
        /// made via interpolation of the Code of the venueand the Start Date of the event.
        /// </summary>
        /// <param name="reference">The <see cref="Venues.Data.Reservation"/>'s 
        /// reference</param>
        /// <returns>A <see cref="ReservationGetDto"/> describing the reservatinon details
        /// if it exists; or a new/blank one if it does not.</returns>
        Task<ReservationGetDto> GetReservation(string reference);

        /// <summary>
        /// Cancel's a <see cref="Venues.Data.Reservation"/> by it's <paramref name="reference"/>.
        /// </summary>
        /// <param name="reference">The <see cref="Venues.Data.Reservation"/>'s
        /// reference.</param>
        /// <returns>True if the Reservation was successfully cancelled;
        /// false otherwise.</returns>
        Task<bool> CancelReservation(string reference);

        /// <summary>
        /// Creates a new reservation from the <paramref name="date"/> and the
        /// <paramref name="venue"/> code.
        /// </summary>
        /// <param name="date">The start date of the Event.</param>
        /// <param name="venue">The <see cref="Venues.Data.Venue.Code"/> of the venue.</param>
        /// <returns>A <see cref="ReservationGetDto"/> describing the details of the new
        /// reservation.</returns>
        Task<ReservationGetDto> CreateReservation(DateTime date, string venue);
        
    }
}
