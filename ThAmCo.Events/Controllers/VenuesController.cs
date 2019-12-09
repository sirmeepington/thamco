using Microsoft.AspNetCore.Mvc;
using ThAmCo.VenuesFacade;

namespace ThAmCo.Events.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IReservation _reservations;

        public VenuesController(IReservation reservations)
        {
            _reservations = reservations;
        }
        


    }
}