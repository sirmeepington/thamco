using System.Collections.Generic;

namespace ThAmCo.Events.Data
{
    public class Staff
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public bool FirstAider { get; set; }

        public List<EventStaff> Events { get; set; }

    }
}