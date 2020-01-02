using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used in viewing the details of a <see cref="Staff"/> member.
    /// </summary>
    public class StaffDetailsViewModel
    {
        /// <inheritdoc cref="Staff.Id" />
        public int Id { get; set; }

        /// <inheritdoc cref="Staff.Name" />
        public string Name { get; set; }

        /// <inheritdoc cref="Staff.Email" />
        public string Email { get; set; }

        /// <summary>
        /// The events that the staff member is assigned to as a 
        /// <see cref="List{T}"/> of <see cref="EventDetailsViewModel"/>s.
        /// </summary>
        public List<EventDetailsViewModel> Events { get; set; }

        /// <inheritdoc cref="Staff.FirstAider" />
        [Display(Name="First Aider")]
        public bool FirstAider { get; set; }
    }
}
