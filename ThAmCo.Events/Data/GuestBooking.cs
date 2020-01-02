using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Events.Data
{
    /// <summary>
    /// A composite object between <see cref="Data.Customer">Event</see> and
    /// <see cref="Data.Event">Staff</see> to resolve a many-to-many 
    /// relationship between the entities.
    /// </summary>
    public class GuestBooking
    {
        /// <summary>
        /// The <see cref="Data.Customer"/>'s Id.
        /// </summary>
        [Key, Column(Order = 1)]
        public int CustomerId { get; set; }

        /// <summary>
        /// The <see cref="Data.Customer"/> that is part of this record.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// The <see cref="Data.Event"/>'s Id.
        /// </summary>
        [Key, Column(Order = 0)]
        public int EventId { get; set; }

        /// <summary>
        /// The <see cref="Data.Event"/> that is part of this record.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// Whether or not the <see cref="Event"/> has been attended by the <see cref="Customer"/>.
        /// </summary>
        public bool Attended { get; set; }
    }
}
