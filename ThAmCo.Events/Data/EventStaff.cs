using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Data
{
    public class EventStaff
    {
        [Key,Column(Order = 0)]
        public int StaffId { get; set; }
        [Key, Column(Order = 1)]
        public int EventId { get; set; }

        public Staff Staff { get; set; }

        public Event Event { get; set; }
    }
}
