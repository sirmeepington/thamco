using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used in the creation of a <see cref="Data.GuestBooking"/>.
    /// </summary>
    public class GuestBookingCreateViewModel
    {

        /// <inheritdoc cref="Data.Customer.Id"/>
        [Key, Column(Order = 1)]
        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; }

        /// <summary>
        /// The <see cref="Data.Customer"/> assigned with this booking.
        /// </summary>
        public Customer Customer { get; set; }

        [Key, Column(Order = 0)]
        [Display(Name = "Event Title")]
        public int EventId { get; set; }

        /// <summary>
        /// The <see cref="Data.Event"/> assigned with this booking.
        /// </summary>
        public Event Event { get; set; }

        /// <inheritdoc cref="Data.GuestBooking.Attended" />
        public bool Attended { get; set; }
    }
}
