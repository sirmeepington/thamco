using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Data
{
    /// <summary>
    /// A composite object between <see cref="Data.Event">Event</see> and
    /// <see cref="Data.Staff">Staff</see> to resolve a many-to-many 
    /// relationship between the entities.
    /// </summary>
    public class EventStaff
    {
        /// <summary>
        /// The <see cref="Data.Staff>Staff</see>'s Id.
        /// </summary>
        [Key,Column(Order = 0)]
        public int StaffId { get; set; }

        /// <summary>
        /// The <see cref="Data.Event>Event</see>'s Id.
        /// </summary>

        [Key, Column(Order = 1)]
        public int EventId { get; set; }

        /// <summary>
        /// The <see cref="Data.Staff"/> member in this record.
        /// </summary>
        public Staff Staff { get; set; }

        /// <summary>
        /// The <see cref="Data.Event"/> in this record.
        /// </summary>
        public Event Event { get; set; }
    }
}
