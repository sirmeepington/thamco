using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class EventReservationViewModel
    {
        public string Reference { get; set; }

        public DateTime EventDate { get; set; }

        public string VenueCode { get; set; }

        [Display(Name = "Reserved at")]
        public DateTime WhenMade { get; set; }

        [Display(Name = "Reserved by:")]
        public string StaffName { get; set; }
    }
}
