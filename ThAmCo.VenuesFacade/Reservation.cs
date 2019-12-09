using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ThAmCo.Venues.Models;

namespace ThAmCo.VenuesFacade
{
    public class Reservation : IReservation
    {
        private ILogger<Reservation> _logger;
        private IConfiguration _config;
        private HttpClient client = null;

        public Reservation(ILogger<Reservation> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        private bool EnsureClient()
        {
            if (client != null)
                return true;

            client = new HttpClient()
            {
                BaseAddress = new Uri(_config["VenuesBaseUrl"]),
                Timeout = TimeSpan.FromSeconds(5)
            };
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            return true;
        }

        public async Task<ReservationGetDto> GetReservation(string venueCode, DateTime startDate)
        {
            if (!EnsureClient())
                return null;

            // ref = $"{availability.VenueCode}{availability.Date:yyyyMMdd}"
            string reference = $"{venueCode}{startDate:yyyyMMdd}";
            ReservationGetDto reservation;
            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["reference"] = reference;
                var response = await client.GetAsync("api/reservation?" + query.ToString());
                response.EnsureSuccessStatusCode();
                string responseStr = await response.Content.ReadAsStringAsync();
                List<ReservationGetDto> reservations = await response.Content.ReadAsAsync<List<ReservationGetDto>>();
                if (reservations.Count > 0)
                    reservation = reservations[0];
                else
                    reservation = new ReservationGetDto();
            } catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when accessing a reservation at reference " + reference + ". Exception: " + ex);
                reservation = new ReservationGetDto();
            }

            return reservation;
        }
    }
}
