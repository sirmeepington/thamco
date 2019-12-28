using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class StaffDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public List<EventDetailsViewModel> Events { get; set; }

        [Display(Name="First Aider")]
        public bool FirstAider { get; set; }
    }
}
