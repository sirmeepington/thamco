using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;
using ThAmCo.Venues.Data;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used to view the details of an <see cref="Data.Event"/>.
    /// </summary>
    public class EventDetailsViewModel
    {
        /// <inheritdoc cref="Data.Event.Id"/>
        public int Id { get; set; }

        /// <inheritdoc cref="Data.Event.Title"/>
        public string Title { get; set; }

        /// <inheritdoc cref="Data.Event.Date"/>
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")] // 12/12/2000 12:00AM
        public DateTime Date { get; set; }

        /// <inheritdoc cref="Data.Event.Duration"/>
        [DisplayFormat(DataFormatString = "{0:hh}h {0:mm}m {0:ss}s")] // 01h 12m 30s
        public TimeSpan? Duration { get; set; }

        /// <inheritdoc cref="Data.Event.TypeId"/>
        [Display(Name = "Event Type")]
        public string TypeId { get; set; }

        /// <summary>
        /// A list of <see cref="GuestBookingDetailsViewModel"/>. <para />
        /// Unlike <see cref="Data.Event.Bookings"/> this uses the details 
        /// view model instead of the raw entity as it is more suited in this case.
        /// </summary>
        public List<GuestBookingDetailsViewModel> Bookings { get; set; }

        /// <summary>
        /// The <see cref="EventWarningType"/> enum value for the event. <para/>
        /// Calculated using <see cref="Controllers.EventsController.GetWarningTypeFromEvent(Event)"/>
        /// </summary>
        public EventWarningType Warnings { get; set; }

        public Dictionary<string,float> Pricings { get; set; }

    }
}
