using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Venues.Data;
using ThAmCo.VenuesFacade;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used in the editing of an <see cref="Data.Event"/>.
    /// </summary>
    public class EventEditViewModel
    {
        /// <inheritdoc cref="Data.Event.Id"/>
        public int Id { get; set; }

        /// <inheritdoc cref="Data.Event.Title"/>
        [Required]
        public string Title { get; set; }

        /// <inheritdoc cref="Data.Event.Duration"/>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// The chosen venue to reserve from the view.
        /// </summary>
        public Availability VenueToReserve { get; set; }

        /// <summary>
        /// A SelectList of <see cref="Availability">Availabilities</see> to choose from within the view.
        /// This SelectList should bind to <see cref="VenueToReserve"/>.
        /// </summary>
        [Display(Name = "Change Venue")]
        public SelectList ApplicableVenues { get; set; }
    }
}