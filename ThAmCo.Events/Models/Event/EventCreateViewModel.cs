using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{

    /// <summary>
    /// A view model to be used in the creation of an <see cref="Event"/>.
    /// </summary>
    public class EventCreateViewModel
    {
        /// <inheritdoc cref="Data.Event.Id"/>
        public int Id { get; set; }

        /// <inheritdoc cref="Data.Event.Title"/>
        [Required]
        public string Title { get; set; }

        /// <inheritdoc cref="Data.Event.Date"/>
        [Required]
        public DateTime Date { get; set; }

        /// <inheritdoc cref="Data.Event.Duration"/>
        public TimeSpan? Duration { get; set; }

        /// <inheritdoc cref="Data.Event.TypeId"/>
        [Required]
        [Display(Name = "Type ID")]
        public string TypeId { get; set; }

        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="EventTypeViewModel"/>s which contains 
        /// valid TypeId's to choose from which have been gathered from the Venues API.
        /// </summary>
        public List<EventTypeViewModel> ValidTypeIds { get; set; }

    }
}
