using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used for viewing the details of a <see cref="Data.GuestBooking"/>.
    /// </summary>
    public class GuestBookingDetailsViewModel
    {
        /// <inheritdoc cref="Data.Customer.Id"/>
        [Key, Column(Order = 1)]
        public int CustomerId { get; set; }

        /// <summary>
        /// The <see cref="Data.Customer"/> assigned to this view model
        /// represented as an <see cref="CustomerDetailsViewModel"/>.
        /// </summary>
        public CustomerDetailsViewModel Customer { get; set; }

        /// <inheritdoc cref="Data.Event.Id"/>
        [Key, Column(Order = 0)]
        public int EventId { get; set; }

        /// <summary>
        /// The <see cref="Data.Event"/> assigned to this view model
        /// represented as an <see cref="EventDetailsViewModel"/>.
        /// </summary>
        public EventDetailsViewModel Event { get; set; }

        /// <inheritdoc cref="Data.GuestBooking.Attended" />
        public bool Attended { get; set; }
    }
}
