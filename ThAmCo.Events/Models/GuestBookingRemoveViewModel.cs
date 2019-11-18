using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class GuestBookingRemoveViewModel
    {
        public int CustomerId { get; set; }

        [Display(Name = "Customer Name")]
        public Customer Customer { get; set; }

        public int EventId { get; set; }

        [Display(Name = "Event Title")]
        public Event Event { get; set; }

        [Display(Name = "Customer Attended?")]
        public bool Attended { get; set; }
    }
}
