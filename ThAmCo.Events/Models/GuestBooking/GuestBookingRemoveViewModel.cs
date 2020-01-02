using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used in the removal of a <see cref="Data.GuestBooking"/>.
    /// </summary>
    public class GuestBookingRemoveViewModel
    {
        /// <inheritdoc cref="Data.Customer.Id"/>
        public int CustomerId { get; set; }

        /// <inheritdoc cref="Data.Customer"/>
        [Display(Name = "Customer Name")]
        public Customer Customer { get; set; }

        /// <inheritdoc cref="Data.Event.Id"/>
        public int EventId { get; set; }

        /// <inheritdoc cref="Data.Event"/>
        [Display(Name = "Event Title")]
        public Event Event { get; set; }

        /// <inheritdoc cref="Data.GuestBooking.Attended" />
        [Display(Name = "Customer Attended?")]
        public bool Attended { get; set; }
    }
}
