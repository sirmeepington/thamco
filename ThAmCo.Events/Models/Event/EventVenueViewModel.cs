using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;
using ThAmCo.Venues.Data;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used in the changing of staff of an <see cref="Data.Event"/>.
    /// </summary>
    public class EventVenueViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")] // 12/12/2000 12:00AM
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh}h {0:mm}m {0:ss}s")] // 01h 12m 30s
        public TimeSpan? Duration { get; set; }

        public string TypeId { get; set; }

        [Display(Name = "Event Type")]
        public string TypeTitle { get; set; }

        public Venue Venue { get; set; }

        public EventReservationViewModel Reservation { get; set; }

        public List<Availability> Availabilities { get; set; }

        public SelectList AvailabilitiesSelectList { get; set; }

        [Display(Name = "Reservation #")]
        public string ReservationReference { get; set; }

        [Display(Name = "Selected Venue")]
        public string SelectedVenue { get; set; }
    }
}
