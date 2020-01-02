using System.Collections.Generic;

namespace ThAmCo.Events.Data
{
    /// <summary>
    /// An object to represent a Staff member.
    /// </summary>
    public class Staff
    {
        /// <summary>
        /// The staff member's Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The e-mail address of the Staff member.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The name of the staff member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether or not the staff member is a first-aider.
        /// </summary>
        public bool FirstAider { get; set; }

        /// <summary>
        /// A list of EventStaff records that connect the event to the staff.
        /// </summary>
        public List<EventStaff> Events { get; set; }

    }
}