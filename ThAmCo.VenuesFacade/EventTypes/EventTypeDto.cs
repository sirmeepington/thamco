
namespace ThAmCo.VenuesFacade.EventTypes
{
    /// <summary>
    /// Data Transfer Object used to access the <see cref="Venues.Data.EventType"/>'s endpoint in the Venues API.
    /// </summary>
    public class EventTypeDto
    {

        /// <summary>
        /// The 3 letter Id of the EventType.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The full name of the EventType.
        /// </summary>
        public string Title { get; set; }

    }
}
