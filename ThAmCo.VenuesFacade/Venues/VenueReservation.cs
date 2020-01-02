using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ThAmCo.Venues.Models;

namespace ThAmCo.VenuesFacade
{
    /// <inheritdoc />
    public class VenueReservation : IVenueReservation
    {
        private ILogger<VenueReservation> _logger;
        private IConfiguration _config;
        private HttpClient _client = null;

        public VenueReservation(ILogger<VenueReservation> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

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
        }

        public async Task<ReservationGetDto> GetReservation(string venueCode, DateTime startDate)
        {
            return await GetReservation($"{venueCode}{startDate:yyyyMMdd}");
        }

        public async Task<ReservationGetDto> GetReservation(string reference)
        {
            EnsureClient();

            ReservationGetDto reservation;
            try
            {
                var response = await _client.GetAsync("api/reservations/" + reference);
                response.EnsureSuccessStatusCode();
                string responseStr = await response.Content.ReadAsStringAsync();
                reservation = await response.Content.ReadAsAsync<ReservationGetDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when accessing a reservation at reference " + reference + ". Exception: " + ex);
                reservation = new ReservationGetDto();
            }

            return reservation;
        }

        public async Task<ReservationGetDto> CreateReservation(DateTime eventDate, string venueCode)
        {
            EnsureClient();

            ReservationPostDto reservationDetails = new ReservationPostDto()
            {
                EventDate = eventDate,
                StaffId = "1",
                VenueCode = venueCode
            };
            ReservationGetDto reservation;
            try { 
                var response = await _client.PostAsJsonAsync("api/reservations", reservationDetails);
                response.EnsureSuccessStatusCode();
                reservation = await response.Content.ReadAsAsync<ReservationGetDto>();
            } catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when creating a reservation. Exception: " + ex);
                reservation = new ReservationGetDto();
            }
            return reservation;
        }

        public async Task<bool> CancelReservation(string reference)
        {
            EnsureClient();

            try
            {
                var response = await _client.DeleteAsync("api/reservations/" + reference);
                response.EnsureSuccessStatusCode();
                return true;
            } catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when cancelling a reservation. Exception: " + ex);
                return false;
            }
        }

        ~VenueReservation()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }

    }
}
