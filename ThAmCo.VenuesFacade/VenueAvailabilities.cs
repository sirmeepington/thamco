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

        public async Task<List<Availability>> GetAvailabilities(string eventType, DateTime from, DateTime to)
        {
            if (!EnsureClient()) {
                return new List<Availability>();
            }
            List<Availability> venue;
            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["eventType"] = eventType;
                query["beginDate"] = from.ToString("o");
                query["endDate"] = to.ToString("o");
                var response = await client.GetAsync("api/availability?"+query.ToString());
                response.EnsureSuccessStatusCode();
                string responseStr = await response.Content.ReadAsStringAsync();
                List<Availability> venues = await response.Content.ReadAsAsync<List<Availability>>();
                venue = venues;
            } catch (HttpRequestException ex)
            {

                _logger.LogError(
                    "Failed to receive venue from Venues server. Exception: " + ex.ToString());
                venue = new List<Availability>();
            }
            return venue;
        }
    }
}
