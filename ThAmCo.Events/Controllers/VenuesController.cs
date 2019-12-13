using Microsoft.AspNetCore.Mvc;
using ThAmCo.VenuesFacade;

namespace ThAmCo.Events.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IVenueReservation _reservations;

        public VenuesController(IVenueReservation reservations)
        {
            _reservations = reservations;
        }
        


    }
}