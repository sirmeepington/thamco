using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model for viewing a general overview of <see cref="Data.Staff"/>.
    /// </summary>
    public class StaffIndexViewModel
    {

        /// <inheritdoc cref="Data.Staff.Id" />
        public int Id { get; set; }

        /// <inheritdoc cref="Data.Staff.Name" />
        public string Name { get; set; }

        /// <inheritdoc cref="Data.Staff.Email" />
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        /// <inheritdoc cref="Data.Staff.FirstAider" />
        [Display(Name = "First Aider")]
        public bool FirstAider { get; set; }
    }
}
