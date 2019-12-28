using System.Collections.Generic;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class EventStaffViewModel
    {
        
        public int Id { get; set; }

        public List<Data.Staff> AvailableStaff { get; set; }

        public int SelectedStaff { get; set; }

        public EventDetailsViewModel Event { get; set; }

        public List<StaffIndexViewModel> AssignedStaff { get; set; }

        public EventWarningType WarningType { get; set; }
    }
}
