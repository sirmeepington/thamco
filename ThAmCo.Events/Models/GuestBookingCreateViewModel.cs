using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class GuestBookingCreateViewModel
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Key, Column(Order = 0)]
        [Display(Name = "Event Title")]
        public int EventId { get; set; }

        public Event Event { get; set; }

        public bool Attended { get; set; }
    }
}
