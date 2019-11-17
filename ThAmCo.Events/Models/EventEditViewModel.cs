using System;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models
{
    public class EventEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public TimeSpan? Duration { get; set; }

    }
}