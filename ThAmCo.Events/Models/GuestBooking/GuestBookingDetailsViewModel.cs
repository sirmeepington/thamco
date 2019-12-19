using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Events.Models
{
    public class GuestBookingDetailsViewModel
    {
        [Key, Column(Order = 1)]
        public int CustomerId { get; set; }

        public CustomerDetailsViewModel Customer { get; set; }

        [Key, Column(Order = 0)]
        public int EventId { get; set; }

        public EventDetailsViewModel Event { get; set; }

        public bool Attended { get; set; }
    }
}
