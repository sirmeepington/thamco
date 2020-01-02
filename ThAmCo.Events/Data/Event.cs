using System;
using System.Collections.Generic;

namespace ThAmCo.Events.Data
{
    /// <summary>
    /// An object representing an event.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The numerical Id for the Event.
        /// In the database this is the identity column.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Title of the event.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The start date of the event.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The duration of the event. 
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// The TypeId of the event.
        /// <see cref="Venues.Data.EventType.Id"/>
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// The <see cref="GuestBooking"/>s that the Event is part of.
        /// </summary>
        public List<GuestBooking> Bookings { get; set; }
        
        /// <summary>
        /// The <see cref="Venues.Data.Reservation.Reference">Reference</see> of the venue reservation.
        /// If there is no reservation this will be null.
        /// </summary>
        public string VenueReservation { get; set; }

        /// <summary>
        /// A list of staff that are part of the event.
        /// </summary>
        public IEnumerable<EventStaff> Staff { get; set; }

        /// <summary>
        /// Whether or not the event has been cancelled and should not be counted towards anything.
        /// </summary>
        public bool Cancelled { get; set; }
    }
}
