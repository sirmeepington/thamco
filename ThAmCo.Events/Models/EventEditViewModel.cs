using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Venues.Data;
using ThAmCo.VenuesFacade;

namespace ThAmCo.Events.Models
{
    public class EventEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public TimeSpan? Duration { get; set; }

        public AvailabilityDto VenueToReserve { get; set; }

        [Display(Name = "Change Venue")]
        public SelectList ApplicableVenues { get; set; }
    }
}