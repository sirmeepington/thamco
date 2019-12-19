using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Events.Data
{
    public class EventStaff 
    {

        public int StaffId;

        public int EventId;

        [ForeignKey("StaffId")]
        public Staff Staff { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}
