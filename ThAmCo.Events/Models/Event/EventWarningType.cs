using System.Collections.Generic;

namespace ThAmCo.Events.Models
{
    [System.Flags]
    public enum EventWarningType
    {
        None = 0,
        NoFirstAider = 1,
        InsufficientStaff = 1 << 1
    }

    public static class EventWarningUtil
    {
        public static bool ContainsWarning(EventWarningType type, EventWarningType secondType)
        {
            return (type & secondType) == type;
        }

        private static readonly Dictionary<EventWarningType,string> Types = new Dictionary<EventWarningType, string>()
        {
            { EventWarningType.None, "" },
            { EventWarningType.NoFirstAider, "No First Aider Specified"},
            { EventWarningType.InsufficientStaff, "Not Enough Staff have been Assigned" }
        };

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