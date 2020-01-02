using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThAmCo.VenuesFacade.EventTypes
{
    /// <inheritdoc cref="IEventTypes"/>
    public class EventTypes : IEventTypes
    {
        private ILogger<EventTypes> _logger;
        private IConfiguration _config;
        private HttpClient _client = null;

        public EventTypes(ILogger<EventTypes> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// Ensures the existance of a client by creating it if it does not exist.
        /// </summary>
        private void EnsureClient()
        {
            if (_client != null)
                return;

            _client = new HttpClient()
            {
                BaseAddress = new Uri(_config["VenuesBaseUrl"]),
                Timeout = TimeSpan.FromSeconds(5)
            };
            _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            _logger.LogDebug("Created a HTTPClient for accessing EventTypes");
        }

        /// <inheritdoc />
        public async Task<EventTypeDto> GetEventType(string type)
        {
            var dtos = await GetEventTypes();
            return dtos.FirstOrDefault(x => x.Id == type);
        }

        /// <inheritdoc />
        public async Task<List<EventTypeDto>> GetEventTypes()
        {
            EnsureClient();

            List<EventTypeDto> eventTypeDtos;
            try
            {
                var response = await _client.GetAsync("api/eventtypes");
                response.EnsureSuccessStatusCode();
                eventTypeDtos = await response.Content.ReadAsAsync<List<EventTypeDto>>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error while getting Event types: " + ex.Message);
                eventTypeDtos = new List<EventTypeDto>();
            }
            return eventTypeDtos;
        }

        /// <summary>
        /// Destructor for disposing the client when necessary to avoid any sort of connection
        /// or memory leaking.
        /// </summary>
        ~EventTypes()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
    }
}
