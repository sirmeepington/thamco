using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using ThAmCo.Venues.Data;

namespace ThAmCo.VenuesFacade
{
    public class VenueAvailabilities : IVenueAvailabilities
    {

        private readonly ILogger<VenueAvailabilities> _logger;
        private readonly IConfiguration _config;

        private HttpClient client = null;

        public VenueAvailabilities(ILogger<VenueAvailabilities> logger, IConfiguration config)
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

<<<<<<< HEAD:ThAmCo.VenuesFacade/VenueAvailabilities.cs
        public async Task<Availability> GetAvailability(string eventType, DateTime from, DateTime to)
        {
            if (!EnsureClient()) {
                return new Availability();
            }
            Availability venue;
=======
        public async Task<List<AvailabilityDto>> GetAvailabilities(string eventType, DateTime from, DateTime to)
        {
            if (!EnsureClient()) {
                return new List<AvailabilityDto>();
            }
            List<AvailabilityDto> venue;
>>>>>>> 07fd791106061d6a5249cba716891de8a6287089:ThAmCo.VenuesFacade/Availabilities.cs
            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["eventType"] = eventType;
<<<<<<< HEAD:ThAmCo.VenuesFacade/VenueAvailabilities.cs
                query["beginDate"] = from.ToString("o");
                query["endDate"] = to.ToString("o");
                var response = await client.GetAsync("api/availability?"+query.ToString());
                response.EnsureSuccessStatusCode();
                string responseStr = await response.Content.ReadAsStringAsync();
                List<Availability> venues = await response.Content.ReadAsAsync<List<Availability>>();
                if (venues.Count > 0)
                    venue = venues[0];
                else
                    venue = new Availability();
=======
                query["beginDate"] = from.ToString("d");
                query["endDate"] = to.ToString("d");
                var response = await client.GetAsync("api/availability?"+query.ToString());
                response.EnsureSuccessStatusCode();
                string responseStr = await response.Content.ReadAsStringAsync();
                List<AvailabilityDto> venues = await response.Content.ReadAsAsync<List<AvailabilityDto>>();
                return venues;
>>>>>>> 07fd791106061d6a5249cba716891de8a6287089:ThAmCo.VenuesFacade/Availabilities.cs
            } catch (HttpRequestException ex)
            {
                _logger.LogError(
                    "Failed to receive venue from Venues server. Exception: " + ex.ToString());
<<<<<<< HEAD:ThAmCo.VenuesFacade/VenueAvailabilities.cs
                venue = new Availability();
=======
                venue = new List<AvailabilityDto>();
>>>>>>> 07fd791106061d6a5249cba716891de8a6287089:ThAmCo.VenuesFacade/Availabilities.cs
            }
            return venue;
        }
    }
}
