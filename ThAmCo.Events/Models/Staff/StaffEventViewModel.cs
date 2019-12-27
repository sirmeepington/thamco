using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models.Staff
{
    public class StaffEventViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<EventDetailsViewModel> Events { get; set; }

        [Display(Name="Is First Aider")]
        public bool FirstAider { get; set; }
    }
}
