using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Events.Data
{
    public class GuestBooking
    {
        [Key, Column(Order = 1)]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Key, Column(Order = 0)]
        public int EventId { get; set; }

        public Event Event { get; set; }

        public bool Attended { get; set; }
    }
}
