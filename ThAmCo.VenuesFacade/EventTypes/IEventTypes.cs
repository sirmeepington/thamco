using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ThAmCo.VenuesFacade.EventTypes
{
    public interface IEventTypes
    {

        Task<EventTypeDto> GetEventType(string type);

    }
}
