using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;
using ThAmCo.Venues.Data;

namespace ThAmCo.Events.Models
{
    public class EventDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")] // 12/12/2000 12:00AM
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh}h {0:mm}m {0:ss}s")] // 01h 12m 30s
        public TimeSpan? Duration { get; set; }

        [Display(Name = "Event Type")]
        public string TypeId { get; set; }

        public List<GuestBookingDetailsViewModel> Bookings { get; set; }

        public EventWarningType Warnings { get; set; }

    }
}
