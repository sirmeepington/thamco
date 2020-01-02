using System.Collections.Generic;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A flaggable enum describing the appicable warning(s) that apply to a certain event.
    /// </summary>
    [System.Flags]
    public enum EventWarningType
    {
        /// <summary>
        /// No warnings.
        /// </summary>
        None = 0,
        /// <summary>
        /// There is no first-aider assigned to the event.
        /// </summary>
        NoFirstAider = 1,
        /// <summary>
        /// There are not enough staff members assigned to the event.
        /// </summary>
        InsufficientStaff = 1 << 1
    }

    /// <summary>
    /// A utility class that contains string definitions for each <see cref="EventWarningType"/>
    /// as well as methods that are used to determine the <see cref="EventWarningType"/> that are
    /// part of the enum value.
    /// </summary>
    public static class EventWarningUtil
    {
        /// <summary>
        /// Checks whether the given <see cref="EventWarningType"/> contains the value given
        /// via <paramref name="secondType"/>.
        /// </summary>
        /// <param name="type">The initial <see cref="EventWarningType"/></param>
        /// <param name="secondType">The <see cref="EventWarningType"/> to check against the first.</param>
        /// <returns>True if the first <see cref="EventWarningType"/> contains the second; false otherwise.</returns>
        public static bool ContainsWarning(EventWarningType type, EventWarningType secondType)
        {
            return (type & secondType) == secondType;
        }

        /// <summary>
        /// A dictionary of <see cref="EventWarningType"/>s mapped to their string definitions.
        /// </summary>
        private static readonly Dictionary<EventWarningType,string> Types = new Dictionary<EventWarningType, string>()
        {
            { EventWarningType.None, string.Empty },
            { EventWarningType.NoFirstAider, "No First Aider Specified"},
            { EventWarningType.InsufficientStaff, "Not Enough Staff have been Assigned" }
        };

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="string"/> definitions for the warnings 
        /// that apply to the given value of <paramref name="type"/>.
        /// <para/>
        /// See also: <seealso cref="EventWarningType"/>
        /// </summary>
        /// <param name="type">The <see cref="EventWarningType"/> given.</param>
        /// <returns>A list of definitions for the warnings in <paramref name="type"/></returns>
        public static List<string> GetWarnings(EventWarningType type)
        {
            List<string> outList = new List<string>();
            foreach(EventWarningType warI in Types.Keys)
            {
                if (ContainsWarning(type, warI))
                {
                    outList.Add(Types[warI]);
                }
            }
            return outList;
        }
    }
}