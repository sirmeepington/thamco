using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using ThAmCo.VenuesFacade.Availabilities;

namespace ThAmCo.VenuesFacade
{
    public class VenueAvailabilities : IVenueAvailabilities
    {

        private readonly ILogger<VenueAvailabilities> _logger;
        private readonly IConfiguration _config;

        private HttpClient _client = null;

        public VenueAvailabilities(ILogger<VenueAvailabilities> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        private bool EnsureClient()
        {
            if (_client != null)
                return true;

            _client = new HttpClient()
            {
                BaseAddress = new Uri(_config["VenuesBaseUrl"]),
                Timeout = TimeSpan.FromSeconds(5)
            };
            _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            return true;
        }

        public async Task<List<AvailabilityApiGetDto>> GetAvailabilities(string eventType, DateTime from, DateTime to)
        {
            if (!EnsureClient()) {
                return new List<AvailabilityApiGetDto>();
            }
            List<AvailabilityApiGetDto> venue;
            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["eventType"] = eventType;
                query["beginDate"] = $"{from:yyyy-MM-dd}";
                query["endDate"] = $"{to:yyyy-MM-dd}";

                var response = await _client.GetAsync("api/availability?"+query.ToString());
                response.EnsureSuccessStatusCode();
                string responseStr = await response.Content.ReadAsStringAsync();
                List<AvailabilityApiGetDto> venues = await response.Content
                    .ReadAsAsync<List<AvailabilityApiGetDto>>();
                venue = venues;
            } catch (HttpRequestException ex)
            {

                _logger.LogError(
                    "Failed to receive venue from Venues server. Exception: " + ex.ToString());
                venue = new List<AvailabilityApiGetDto>();
            }
            return venue;
        }

        ~VenueAvailabilities()
        {
            _client.Dispose();
            _client = null;
        }
    }
}
