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
    public class Availabilities : IAvailabilities
    {

        private readonly ILogger<Availabilities> _logger;
        private readonly IConfiguration _config;

        private HttpClient client = null;

        public Availabilities(ILogger<Availabilities> logger, IConfiguration config)
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

        public async Task<List<AvailabilityDto>> GetAvailabilities(string eventType, DateTime from, DateTime to)
        {
            if (!EnsureClient()) {
                return new List<AvailabilityDto>();
            }
            List<AvailabilityDto> venue;
            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["eventType"] = eventType;
                query["beginDate"] = from.ToString("d");
                query["endDate"] = to.ToString("d");
                var response = await client.GetAsync("api/availability?"+query.ToString());
                response.EnsureSuccessStatusCode();
                string responseStr = await response.Content.ReadAsStringAsync();
                List<AvailabilityDto> venues = await response.Content.ReadAsAsync<List<AvailabilityDto>>();
                return venues;
            } catch (HttpRequestException ex)
            {
                _logger.LogError(
                    "Failed to receive venue from Venues server. Exception: " + ex.ToString());
                venue = new List<AvailabilityDto>();
            }
            return venue;
        }
    }
}
