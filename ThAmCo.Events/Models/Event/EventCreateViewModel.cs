﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class EventCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        [Required]
        [Display(Name = "Type ID")]
        public string TypeId { get; set; }

        public List<EventTypeViewModel> ValidTypeIds { get; set; }

    }
}
