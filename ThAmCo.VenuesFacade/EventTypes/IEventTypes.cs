using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ThAmCo.VenuesFacade.EventTypes
{
    /// <summary>
    /// Methods for interacting with the EventTypes endpoint of the Venues API.
    /// </summary>
    public interface IEventTypes
    {

        /// <summary>
        /// Gets a specific EventType by its <see cref="Venues.Data.EventType.Id"/>
        /// </summary>
        /// <param name="type">The <see cref="Venues.Data.EventType"/>'s <see cref="Venues.Data.EventType.Id"/></param>
        /// <returns>An <see cref="EventTypeDto"/> representing the EventType</returns>
        Task<EventTypeDto> GetEventType(string type);

        /// <summary>
        /// Gets a <see cref="List{EventTypeDto}"/> (T is <see cref="EventTypeDto"/>) representing 
        /// all available event types.
        /// </summary>
        /// <returns>A list of all EventTypes.</returns>
        Task<List<EventTypeDto>> GetEventTypes();
    }
}
