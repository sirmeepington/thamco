using System.Collections.Generic;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used in the changing of staff of an <see cref="Data.Event"/>.
    /// </summary>
    public class EventStaffViewModel
    {
        /// <inheritdoc cref="Data.Event.Id"/>
        public int Id { get; set; }

        /// <summary>
        /// A list of Staff that are available to be added to the <see cref="Data.Event"/>.
        /// </summary>
        public List<Data.Staff> AvailableStaff { get; set; }

        /// <summary>
        /// The Id of the currently selected staff member. This is populated during
        /// the view via a SelectList.
        /// </summary>
        public int SelectedStaff { get; set; }

        /// <summary>
        /// The <see cref="EventDetailsViewModel"/> describing the current event.
        /// </summary>
        public EventDetailsViewModel Event { get; set; }

        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="StaffIndexViewModel"/> which shows staff that are
        /// assigned to the current event.
        /// </summary>
        public List<StaffIndexViewModel> AssignedStaff { get; set; }

        /// <inheritdoc cref="EventDetailsViewModel.Warnings" />
        public EventWarningType WarningType { get; set; }
    }
}
