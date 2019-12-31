using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models
{
    public class EventDeleteViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        [Display(Name = "Event Type")]
        public string TypeId { get; set; }

        public bool Cancelled { get; set; }

        public string VenueReservation { get; set; }

    }
}
