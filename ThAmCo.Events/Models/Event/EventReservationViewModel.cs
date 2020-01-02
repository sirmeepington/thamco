using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used in the reservation of venue for an <see cref="Data.Event"/>.
    /// </summary>
    public class EventReservationViewModel
    {
        /// <inheritdoc cref="Venues.Data.Reservation"/>
        public string Reference { get; set; }

        /// <inheritdoc cref="Venues.Data.Reservation.WhenMade"/>
        public DateTime EventDate { get; set; }

        /// <inheritdoc cref="Venues.Data.Reservation.VenueCode" />
        public string VenueCode { get; set; }

        /// <inheritdoc cref="Venues.Data.Reservation.WhenMade" />
        [Display(Name = "Reserved at")]
        public DateTime WhenMade { get; set; }

        /// <inheritdoc cref="Venues.Data.Reservation.StaffId />
        [Display(Name = "Reserved by:")]
        public string StaffName { get; set; }
    }
}
