using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class StaffIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "First Aider")]
        public bool FirstAider { get; set; }
    }
}
