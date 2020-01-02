using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used in the deletion of an <see cref="Data.Event"/>.
    /// </summary>
    public class EventDeleteViewModel
    {
        /// <inheritdoc cref="Data.Event.Id"/>
        public int Id { get; set; }

        /// <inheritdoc cref="Data.Event.Title"/>
        public string Title { get; set; }

        /// <inheritdoc cref="Data.Event.Date"/>
        public DateTime Date { get; set; }

        /// <inheritdoc cref="Data.Event.Duration"/>
        public TimeSpan? Duration { get; set; }

        /// <inheritdoc cref="Data.Event.TypeId"/>
        [Display(Name = "Event Type")]
        public string TypeId { get; set; }

        /// <inheritdoc cref="Data.Event.Cancelled"/>
        public bool Cancelled { get; set; }

        /// <inheritdoc cref="Data.Event.VenueReservation"/>
        public string VenueReservation { get; set; }

    }
}
